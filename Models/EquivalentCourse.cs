namespace PUQAMS.Models;

/// <summary>
/// Maps a course to another course that is treated as equivalent to it
/// (for example, the same course under an older curriculum version).
/// One course (CourseId) can have several equivalent courses, so this
/// is a self-referencing many-to-many pivot on Course.
/// </summary>
public class EquivalentCourse
{
    public int Id { get; set; }

    // The "owner" course (oCourse in the API response).
    public int CourseId { get; set; }

    // The course that is equivalent to it.
    public int EquivalentCourseId { get; set; }

    public bool IsActive { get; set; } = true;

    public Course Course { get; set; } = null!;

    public Course EquivalentCourseNav { get; set; } = null!;
}
