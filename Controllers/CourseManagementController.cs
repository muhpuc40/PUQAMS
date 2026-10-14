using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;
using PUQAMS.Dtos;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseManagementController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public CourseManagementController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: /api/CourseManagement/get_clos_by_course?course_id=773&program_id=1
    //
    // Returns every active CLO of the given course. Each CLO can map to
    // more than one PLO, returned as a comma-separated list of PLO short
    // names (e.g. "PLO2,PLO3,PLO5") in the "plos" field.
    [HttpGet("get_clos_by_course")]
    public async Task<IActionResult> GetClosByCourse(
        [FromQuery(Name = "course_id")] int courseId,
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
    {
        if (courseId <= 0 || programId <= 0)
        {
            return BadRequest(new ApiResponse<object>(
                1400, "course_id and program_id are required.", null));
        }

        // The course must belong to the given program.
        var course = await _dbContext.Courses
            .AsNoTracking()
            .Where(x =>
                x.Id == courseId &&
                x.IsActive &&
                x.Version.ProgramId == programId &&
                x.Version.IsActive &&
                x.Version.Program.IsActive)
            .Select(x => new
            {
                x.Id,
                x.CourseCode,
                x.CourseName,
                x.CourseCredit
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (course is null)
        {
            return NotFound(new ApiResponse<object>(
                1404, "Course was not found for this program.", null));
        }

        var clos = await _dbContext.Clos
            .AsNoTracking()
            .Where(x => x.CourseId == course.Id && x.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => new
            {
                x.Id,
                x.CourseOutcome,
                x.Description
            })
            .ToListAsync(cancellationToken);

        // Every (CLO, PLO) link for this course's CLOs, flattened, so the
        // "PLO2,PLO3,PLO5" string can be built in memory afterwards.
        var ploLinks = await _dbContext.CloPlos
            .AsNoTracking()
            .Where(x => x.Clo.CourseId == course.Id && x.Clo.IsActive)
            .OrderBy(x => x.PloId)
            .Select(x => new { x.CloId, PloShortName = x.Plo.ShortName })
            .ToListAsync(cancellationToken);

        var plosByCloId = ploLinks
            .GroupBy(x => x.CloId)
            .ToDictionary(
                g => g.Key,
                g => string.Join(",", g.Select(x => x.PloShortName)));

        var courseLabel =
            $"{course.CourseCode} - {course.CourseName} " +
            $"({FormatCredit(course.CourseCredit)})";

        var data = clos
            .Select(x => new CloDto
            {
                CourseOutcome = x.CourseOutcome,
                Description = x.Description,
                Course = courseLabel,
                TblCourseId = course.Id,
                Id = x.Id,
                Plos = plosByCloId.TryGetValue(x.Id, out var joined)
                    ? joined
                    : ""
            })
            .ToList();

        return Ok(new ApiResponse<List<CloDto>>(1200, "Success", data));
    }

    // GET: /api/CourseManagement/mdr_list?department_id=1&coursetype_id=2
    //
    // Lists the mark-distribution rule names defined for a department and
    // course type (e.g. the Lab rules of CSE, or the Theory rules of CSE).
    [HttpGet("mdr_list")]
    public async Task<IActionResult> GetMdRuleList(
        [FromQuery(Name = "department_id")] int departmentId,
        [FromQuery(Name = "coursetype_id")] int courseTypeId,
        CancellationToken cancellationToken)
    {
        if (departmentId <= 0 || courseTypeId <= 0)
        {
            return BadRequest(new ApiResponse<object>(
                1400, "department_id and coursetype_id are required.", null));
        }

        var departmentExists = await _dbContext.Departments
            .AsNoTracking()
            .AnyAsync(x => x.Id == departmentId && x.IsActive, cancellationToken);

        if (!departmentExists)
        {
            return NotFound(new ApiResponse<object>(
                1404, "Department was not found.", null));
        }

        var rules = await _dbContext.MdRuleNames
            .AsNoTracking()
            .Where(x =>
                x.DepartmentId == departmentId &&
                x.CourseTypeId == courseTypeId &&
                x.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => new MdRuleNameDto
            {
                RuleName = x.RuleName,
                Id = x.Id
            })
            .ToListAsync(cancellationToken);

        return Ok(new ApiResponse<List<MdRuleNameDto>>(1200, "Success", rules));
    }

    // GET: /api/CourseManagement/mdr_details?rule_id=1&department_id=1
    //
    // Lists the mark-distribution line items (Attendance, Mid Term, ...)
    // of one rule. department_id confirms the rule belongs to that
    // department, the same way version_id/program_id is validated on
    // CourseController's endpoints.
    [HttpGet("mdr_details")]
    public async Task<IActionResult> GetMdRuleDetails(
        [FromQuery(Name = "rule_id")] int ruleId,
        [FromQuery(Name = "department_id")] int departmentId,
        CancellationToken cancellationToken)
    {
        if (ruleId <= 0 || departmentId <= 0)
        {
            return BadRequest(new ApiResponse<object>(
                1400, "rule_id and department_id are required.", null));
        }

        var ruleExists = await _dbContext.MdRuleNames
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id == ruleId &&
                x.DepartmentId == departmentId &&
                x.IsActive,
                cancellationToken);

        if (!ruleExists)
        {
            return NotFound(new ApiResponse<object>(
                1404, "Rule was not found for this department.", null));
        }

        var details = await _dbContext.MdRuleDetails
            .AsNoTracking()
            .Where(x => x.RuleId == ruleId && x.IsActive)
            .OrderBy(x => x.Ordering)
            .Select(x => new MdRuleDetailDto
            {
                Id = x.Id,
                SessionCourseColumn = x.SessionCourseColumn,
                DisplayName = x.DisplayName,
                MaxMark = x.MaxMark,
                InRetake = x.InRetake ? "yes" : "no",
                AllowA = x.AllowA ? 1 : 0,
                AllowExp = x.AllowExp ? 1 : 0,
                FailIfA = x.FailIfA ? 1 : 0,
                FailIfExp = x.FailIfExp ? 1 : 0,
                Ordering = x.Ordering
            })
            .ToListAsync(cancellationToken);

        return Ok(new ApiResponse<List<MdRuleDetailDto>>(1200, "Success", details));
    }

    // 1.5 stays "1.5", 3.0 becomes "3" (matches the existing course_hr /
    // coursecredit style elsewhere in the API).
    private static string FormatCredit(decimal credit)
        => credit.ToString("0.##");
}
