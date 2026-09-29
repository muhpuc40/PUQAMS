namespace PUQAMS.Models;

/// <summary>
/// A course of one curriculum version.
/// </summary>
public class Course
{
    public int Id { get; set; }

    public int VersionId { get; set; }

    // Example: CSE 1113(V4)
    public string CourseCode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    // 1.5, 2, 3 ...
    public decimal CourseCredit { get; set; }

    // Theory | Lab
    public string CourseType { get; set; } = "Theory";

    public string ShortName { get; set; } = string.Empty;

    // Semester in which the course is offered (1, 2, 3 ...)
    public int ExSemester { get; set; }

    // Contact hours. Null when not set.
    public int? CourseHour { get; set; }

    public string MajorName { get; set; } = string.Empty;

    public string TranscriptCourseCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public CourseVersion Version { get; set; } = null!;
}
