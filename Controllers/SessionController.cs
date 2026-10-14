using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;
using PUQAMS.Dtos;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public SessionController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: /api/Session/get_session_categories
    [HttpGet("get_session_categories")]
    public async Task<IActionResult> GetSessionCategories(CancellationToken cancellationToken)
    {
        var categories = await _dbContext.SessionCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => new SessionCategoryDto { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return Ok(new ApiResponse<List<SessionCategoryDto>>(1200, "Success", categories));
    }

    // GET: /api/Session/get_actual_session
    [HttpGet("get_actual_session")]
    public async Task<IActionResult> GetActualSession(CancellationToken cancellationToken)
    {
        var sessions = await _dbContext.ActualSessions
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => new ActualSessionDto { SessionName = x.SessionName })
            .ToListAsync(cancellationToken);

        return Ok(new ApiResponse<List<ActualSessionDto>>(1200, "Success", sessions));
    }

    // GET: /api/Session/get_session?program_id=1&status=All
    // GET: /api/Session/get_session?program_id=1&status=Running
    // GET: /api/Session/get_session?program_id=1&status=Completed
    //
    // status=All (or anything other than "Running"/"Completed") returns
    // every active session of the program, newest first.
    [HttpGet("get_session")]
    public async Task<IActionResult> GetSession(
        [FromQuery(Name = "program_id")] int programId,
        [FromQuery(Name = "status")] string status = "All",
        CancellationToken cancellationToken = default)
    {
        if (programId <= 0)
        {
            return BadRequest(new ApiResponse<object>(
                1400, "program_id is required.", null));
        }

        var programExists = await _dbContext.AcademicPrograms
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id == programId &&
                x.IsActive &&
                x.Department.IsActive,
                cancellationToken);

        if (!programExists)
        {
            return NotFound(new ApiResponse<object>(
                1404, "Program was not found.", null));
        }

        var query = _dbContext.Sessions
            .AsNoTracking()
            .Where(x => x.ProgramId == programId && x.IsActive);

        if (string.Equals(status, "Running", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(x => x.Status == status);
        }
        // Any other value (e.g. "All", missing, blank) returns every status.

        var rows = await query
            .OrderByDescending(x => x.StartDate)
            .ThenByDescending(x => x.Id)
            .Select(x => new
            {
                x.Id,
                x.SessionName,
                x.StartDate,
                x.EndDate,
                x.IsEnrollmentOpen,
                x.IsEnrollmentOpenStu,
                x.ExamName,
                x.ExamHeldIn,
                x.IsRoutinePublished,
                x.Status,
                x.IsCreditTransfer
            })
            .ToListAsync(cancellationToken);

        var data = rows
            .Select(x => new SessionDto
            {
                Id = x.Id,
                SessionName = x.SessionName,
                StartDate = x.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndDate = x.EndDate.HasValue
                    ? x.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss")
                    : "",
                IsEnrollmentOpen = x.IsEnrollmentOpen ? 1 : 0,
                IsEnrollmentOpenStu = x.IsEnrollmentOpenStu ? 1 : 0,
                ExamName = x.ExamName,
                ExamHeldIn = x.ExamHeldIn,
                IsRoutinePublished = x.IsRoutinePublished ? 1 : 0,
                Status = x.Status,
                IsCreditTransfer = x.IsCreditTransfer ? 1 : (object)""
            })
            .ToList();

        return Ok(new ApiResponse<List<SessionDto>>(1200, "Success", data));
    }
}
