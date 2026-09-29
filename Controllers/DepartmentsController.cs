using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;

namespace PUQAMS.Controllers;

// Public on purpose: the login screen needs the department list
// before the user has a token.
[ApiController]
[Route("api/v1/departments")]
[AllowAnonymous]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public DepartmentsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: /api/v1/departments
    [HttpGet]
    public async Task<IActionResult> GetDepartments(
        CancellationToken cancellationToken)
    {
        var departments =
            await _dbContext.Departments
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name
                })
                .ToListAsync(cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Departments retrieved successfully.",
            data = departments
        });
    }

    // GET: /api/v1/departments/{departmentId}/programs
    [HttpGet("{departmentId:int}/programs")]
    public async Task<IActionResult> GetProgramsByDepartment(
        int departmentId,
        CancellationToken cancellationToken)
    {
        var department =
            await _dbContext.Departments
                .AsNoTracking()
                .Where(x => x.Id == departmentId && x.IsActive)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Department was not found."
            });
        }

        var programs =
            await _dbContext.AcademicPrograms
                .AsNoTracking()
                .Where(x => x.DepartmentId == departmentId && x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    departmentId = x.DepartmentId,
                    code = x.Code,
                    name = x.Name,
                    shortName = x.ShortName
                })
                .ToListAsync(cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Programs retrieved successfully.",
            department,
            data = programs
        });
    }

    // GET: /api/v1/departments/tree
    [HttpGet("tree")]
    public async Task<IActionResult> GetDepartmentProgramTree(
        CancellationToken cancellationToken)
    {
        var departments =
            await _dbContext.Departments
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,

                    programs = x.Programs
                        .Where(program => program.IsActive)
                        .OrderBy(program => program.SortOrder)
                        .ThenBy(program => program.Name)
                        .Select(program => new
                        {
                            id = program.Id,
                            departmentId = program.DepartmentId,
                            code = program.Code,
                            name = program.Name,
                            shortName = program.ShortName
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Department and program tree retrieved successfully.",
            data = departments
        });
    }
}
