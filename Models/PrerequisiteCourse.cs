namespace PUQAMS.Models;

/// <summary>
/// Maps a course to another course that must be completed before it.
/// One course (CourseId) can have several prerequisite courses, so this
/// is a self-referencing many-to-many pivot on Course.
/// </summary>
public class PrerequisiteCourse
{
    public int Id { get; set; }

    // The "owner" course (oCourse in the API response) that requires
    // the prerequisite.
    public int CourseId { get; set; }

    // The course that must be completed first.
    public int PrerequisiteCourseId { get; set; }

    public bool IsActive { get; set; } = true;

    public Course Course { get; set; } = null!;

    public Course PrerequisiteCourseNav { get; set; } = null!;
}
