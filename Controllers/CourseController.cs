using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;
using PUQAMS.Dtos;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public CourseController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: /api/Course/get_programwise_course_version?program_id=1
    [HttpGet("get_programwise_course_version")]
    public async Task<IActionResult> GetProgramWiseCourseVersion(
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
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

        var versions = await _dbContext.CourseVersions
            .AsNoTracking()
            .Where(x => x.ProgramId == programId && x.IsActive)
            .OrderBy(x => x.VersionNumber)
            .Select(x => new CourseVersionDto
            {
                Id = x.Id,
                VersionName = x.Name,
                Department = x.Program.Department.Code,
                DepartmentFullName = x.Program.Department.Name,
                Program = x.Program.Level,
                DegreeFullName = x.Program.Name,
                ProgramAdmitCard = x.Program.ShortName
            })
            .ToListAsync(cancellationToken);

        return Ok(new ApiResponse<List<CourseVersionDto>>(
            1200, "Success", versions));
    }

    // GET: /api/Course/get_versionwise_course_list?version_id=12&program_id=1
    [HttpGet("get_versionwise_course_list")]
    public async Task<IActionResult> GetVersionWiseCourseList(
        [FromQuery(Name = "version_id")] int versionId,
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
    {
        var programLevel = await GetValidatedVersionProgramLevelAsync(
            versionId, programId, cancellationToken);

        if (programLevel is null)
        {
            return VersionNotFound();
        }

        var rows = await _dbContext.Courses
            .AsNoTracking()
            .Where(x => x.VersionId == versionId && x.IsActive)
            .OrderBy(x => x.ExSemester)
            .ThenBy(x => x.CourseCode)
            .Select(x => new
            {
                x.Id,
                x.CourseCode,
                x.CourseName,
                x.CourseCredit,
                x.CourseType,
                x.ShortName,
                x.ExSemester,
                x.CourseHour,
                x.MajorName,
                x.TranscriptCourseCode
            })
            .ToListAsync(cancellationToken);

        var courses = rows
            .Select(x => BuildCourseDto(
                x.Id, x.CourseCode, x.CourseName, x.CourseCredit,
                x.CourseType, x.ShortName, x.ExSemester, programLevel,
                x.CourseHour, x.MajorName, x.TranscriptCourseCode))
            .ToList();

        return Ok(new ApiResponse<List<CourseDto>>(
            1200, "Success", courses));
    }

    // GET: /api/Course/get_equivalent_courses?version_id=12&program_id=1
    //
    // For every course of the given version that has at least one
    // equivalent course, returns the course itself (oCourse) together
    // with the list of courses that are equivalent to it. A course can
    // have more than one equivalent course.
    [HttpGet("get_equivalent_courses")]
    public async Task<IActionResult> GetEquivalentCourses(
        [FromQuery(Name = "version_id")] int versionId,
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
    {
        if (await GetValidatedVersionProgramLevelAsync(
                versionId, programId, cancellationToken) is null)
        {
            return VersionNotFound();
        }

        var rows = await _dbContext.EquivalentCourses
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.Course.VersionId == versionId &&
                x.Course.IsActive &&
                x.EquivalentCourseNav.IsActive)
            .OrderBy(x => x.Course.ExSemester)
            .ThenBy(x => x.Course.CourseCode)
            .ThenBy(x => x.EquivalentCourseNav.CourseCode)
            .Select(x => new RelatedCourseRow
            {
                OwnerId = x.Course.Id,
                OwnerCode = x.Course.CourseCode,
                OwnerName = x.Course.CourseName,
                OwnerCredit = x.Course.CourseCredit,
                OwnerType = x.Course.CourseType,
                OwnerShortName = x.Course.ShortName,
                OwnerSemester = x.Course.ExSemester,
                OwnerHour = x.Course.CourseHour,
                OwnerMajor = x.Course.MajorName,
                OwnerTranscript = x.Course.TranscriptCourseCode,
                OwnerProgramLevel = x.Course.Version.Program.Level,

                TargetId = x.EquivalentCourseNav.Id,
                TargetCode = x.EquivalentCourseNav.CourseCode,
                TargetName = x.EquivalentCourseNav.CourseName,
                TargetCredit = x.EquivalentCourseNav.CourseCredit,
                TargetType = x.EquivalentCourseNav.CourseType,
                TargetShortName = x.EquivalentCourseNav.ShortName,
                TargetSemester = x.EquivalentCourseNav.ExSemester,
                TargetHour = x.EquivalentCourseNav.CourseHour,
                TargetMajor = x.EquivalentCourseNav.MajorName,
                TargetTranscript = x.EquivalentCourseNav.TranscriptCourseCode,
                TargetProgramLevel = x.EquivalentCourseNav.Version.Program.Level
            })
            .ToListAsync(cancellationToken);

        var data = GroupRows(rows)
            .Select(g => new EquivalentCourseGroupDto
            {
                OCourse = g.OCourse,
                EquivalentCourse = g.RelatedCourses
            })
            .ToList();

        return Ok(new ApiResponse<List<EquivalentCourseGroupDto>>(
            1200, "Success", data));
    }

    // GET: /api/Course/get_prerequisite_courses?version_id=12&program_id=1
    //
    // For every course of the given version that has at least one
    // prerequisite course, returns the course itself (oCourse) together
    // with the list of courses required before it. A course can have
    // more than one prerequisite course.
    [HttpGet("get_prerequisite_courses")]
    public async Task<IActionResult> GetPrerequisiteCourses(
        [FromQuery(Name = "version_id")] int versionId,
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
    {
        if (await GetValidatedVersionProgramLevelAsync(
                versionId, programId, cancellationToken) is null)
        {
            return VersionNotFound();
        }

        var rows = await _dbContext.PrerequisiteCourses
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.Course.VersionId == versionId &&
                x.Course.IsActive &&
                x.PrerequisiteCourseNav.IsActive)
            .OrderBy(x => x.Course.ExSemester)
            .ThenBy(x => x.Course.CourseCode)
            .ThenBy(x => x.PrerequisiteCourseNav.CourseCode)
            .Select(x => new RelatedCourseRow
            {
                OwnerId = x.Course.Id,
                OwnerCode = x.Course.CourseCode,
                OwnerName = x.Course.CourseName,
                OwnerCredit = x.Course.CourseCredit,
                OwnerType = x.Course.CourseType,
                OwnerShortName = x.Course.ShortName,
                OwnerSemester = x.Course.ExSemester,
                OwnerHour = x.Course.CourseHour,
                OwnerMajor = x.Course.MajorName,
                OwnerTranscript = x.Course.TranscriptCourseCode,
                OwnerProgramLevel = x.Course.Version.Program.Level,

                TargetId = x.PrerequisiteCourseNav.Id,
                TargetCode = x.PrerequisiteCourseNav.CourseCode,
                TargetName = x.PrerequisiteCourseNav.CourseName,
                TargetCredit = x.PrerequisiteCourseNav.CourseCredit,
                TargetType = x.PrerequisiteCourseNav.CourseType,
                TargetShortName = x.PrerequisiteCourseNav.ShortName,
                TargetSemester = x.PrerequisiteCourseNav.ExSemester,
                TargetHour = x.PrerequisiteCourseNav.CourseHour,
                TargetMajor = x.PrerequisiteCourseNav.MajorName,
                TargetTranscript = x.PrerequisiteCourseNav.TranscriptCourseCode,
                TargetProgramLevel = x.PrerequisiteCourseNav.Version.Program.Level
            })
            .ToListAsync(cancellationToken);

        var data = GroupRows(rows)
            .Select(g => new PrerequisiteCourseGroupDto
            {
                OCourse = g.OCourse,
                PrerequisiteCourse = g.RelatedCourses
            })
            .ToList();

        return Ok(new ApiResponse<List<PrerequisiteCourseGroupDto>>(
            1200, "Success", data));
    }

    // GET: /api/Course/get_dominant_courses?version_id=12&program_id=1
    //
    // For every course of the given version that has at least one
    // dominant course, returns the course itself (oCourse) together
    // with the list of courses that dominate/exempt it. A course can
    // have more than one dominant course.
    [HttpGet("get_dominant_courses")]
    public async Task<IActionResult> GetDominantCourses(
        [FromQuery(Name = "version_id")] int versionId,
        [FromQuery(Name = "program_id")] int programId,
        CancellationToken cancellationToken)
    {
        if (await GetValidatedVersionProgramLevelAsync(
                versionId, programId, cancellationToken) is null)
        {
            return VersionNotFound();
        }

        var rows = await _dbContext.DominantCourses
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.Course.VersionId == versionId &&
                x.Course.IsActive &&
                x.DominantCourseNav.IsActive)
            .OrderBy(x => x.Course.ExSemester)
            .ThenBy(x => x.Course.CourseCode)
            .ThenBy(x => x.DominantCourseNav.CourseCode)
            .Select(x => new RelatedCourseRow
            {
                OwnerId = x.Course.Id,
                OwnerCode = x.Course.CourseCode,
                OwnerName = x.Course.CourseName,
                OwnerCredit = x.Course.CourseCredit,
                OwnerType = x.Course.CourseType,
                OwnerShortName = x.Course.ShortName,
                OwnerSemester = x.Course.ExSemester,
                OwnerHour = x.Course.CourseHour,
                OwnerMajor = x.Course.MajorName,
                OwnerTranscript = x.Course.TranscriptCourseCode,
                OwnerProgramLevel = x.Course.Version.Program.Level,

                TargetId = x.DominantCourseNav.Id,
                TargetCode = x.DominantCourseNav.CourseCode,
                TargetName = x.DominantCourseNav.CourseName,
                TargetCredit = x.DominantCourseNav.CourseCredit,
                TargetType = x.DominantCourseNav.CourseType,
                TargetShortName = x.DominantCourseNav.ShortName,
                TargetSemester = x.DominantCourseNav.ExSemester,
                TargetHour = x.DominantCourseNav.CourseHour,
                TargetMajor = x.DominantCourseNav.MajorName,
                TargetTranscript = x.DominantCourseNav.TranscriptCourseCode,
                TargetProgramLevel = x.DominantCourseNav.Version.Program.Level
            })
            .ToListAsync(cancellationToken);

        var data = GroupRows(rows)
            .Select(g => new DominantCourseGroupDto
            {
                OCourse = g.OCourse,
                DominantCourse = g.RelatedCourses
            })
            .ToList();

        return Ok(new ApiResponse<List<DominantCourseGroupDto>>(
            1200, "Success", data));
    }

    // -----------------------------------------------------------------
    // Shared helpers
    // -----------------------------------------------------------------

    /// <summary>
    /// Confirms the version belongs to the given program and is active.
    /// Returns the program's level (Undergraduate/Postgraduate/Diploma),
    /// or null if the version/program combination is invalid.
    /// </summary>
    private async Task<string?> GetValidatedVersionProgramLevelAsync(
        int versionId,
        int programId,
        CancellationToken cancellationToken)
    {
        if (versionId <= 0 || programId <= 0)
        {
            return null;
        }

        return await _dbContext.CourseVersions
            .AsNoTracking()
            .Where(x =>
                x.Id == versionId &&
                x.ProgramId == programId &&
                x.IsActive)
            .Select(x => x.Program.Level)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private IActionResult VersionNotFound()
        => NotFound(new ApiResponse<object>(
            1404, "Version was not found for this program.", null));

    /// <summary>
    /// A single flat (owner course, related course) pair as it comes back
    /// from the database. Kept flat on purpose: EF Core can translate a
    /// projection like this straight into one SQL query with two joins,
    /// while building CourseDto objects (with the number-or-"" course_hr
    /// rule) has to happen afterwards, in memory.
    /// </summary>
    private sealed class RelatedCourseRow
    {
        public int OwnerId { get; set; }
        public string OwnerCode { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public decimal OwnerCredit { get; set; }
        public string OwnerType { get; set; } = "";
        public string OwnerShortName { get; set; } = "";
        public int OwnerSemester { get; set; }
        public int? OwnerHour { get; set; }
        public string OwnerMajor { get; set; } = "";
        public string OwnerTranscript { get; set; } = "";
        public string OwnerProgramLevel { get; set; } = "";

        public int TargetId { get; set; }
        public string TargetCode { get; set; } = "";
        public string TargetName { get; set; } = "";
        public decimal TargetCredit { get; set; }
        public string TargetType { get; set; } = "";
        public string TargetShortName { get; set; } = "";
        public int TargetSemester { get; set; }
        public int? TargetHour { get; set; }
        public string TargetMajor { get; set; } = "";
        public string TargetTranscript { get; set; } = "";
        public string TargetProgramLevel { get; set; } = "";
    }

    private sealed record GroupedCourseRows(
        CourseDto OCourse,
        List<CourseDto> RelatedCourses);

    /// <summary>
    /// Groups flat (owner, related) rows by owner course and converts
    /// every side into a CourseDto. Runs in memory, after the database
    /// query has already returned.
    /// </summary>
    private static List<GroupedCourseRows> GroupRows(
        List<RelatedCourseRow> rows)
    {
        return rows
            .GroupBy(x => x.OwnerId)
            .Select(g =>
            {
                var first = g.First();

                var oCourse = BuildCourseDto(
                    first.OwnerId, first.OwnerCode, first.OwnerName,
                    first.OwnerCredit, first.OwnerType, first.OwnerShortName,
                    first.OwnerSemester, first.OwnerProgramLevel,
                    first.OwnerHour, first.OwnerMajor, first.OwnerTranscript);

                var relatedCourses = g
                    .Select(x => BuildCourseDto(
                        x.TargetId, x.TargetCode, x.TargetName,
                        x.TargetCredit, x.TargetType, x.TargetShortName,
                        x.TargetSemester, x.TargetProgramLevel,
                        x.TargetHour, x.TargetMajor, x.TargetTranscript))
                    .ToList();

                return new GroupedCourseRows(oCourse, relatedCourses);
            })
            .ToList();
    }

    /// <summary>
    /// Builds a CourseDto in memory. course_hr comes out as a number when
    /// set, or "" when not, matching the existing API shape.
    /// </summary>
    private static CourseDto BuildCourseDto(
        int id,
        string courseCode,
        string courseName,
        decimal courseCredit,
        string courseType,
        string shortName,
        int exSemester,
        string programLevel,
        int? courseHour,
        string majorName,
        string transcriptCourseCode)
    {
        return new CourseDto
        {
            Id = id,
            CourseCode = courseCode,
            CourseName = courseName,
            CourseCredit = (double)courseCredit,
            CourseType = courseType,
            ShortName = shortName,
            ExSemester = exSemester,
            Program = programLevel,
            CourseHr = courseHour.HasValue ? (object)courseHour.Value : "",
            MajorName = majorName,
            TranscriptCourseCode = transcriptCourseCode
        };
    }
}
