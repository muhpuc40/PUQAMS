namespace PUQAMS.Models;

/// <summary>
/// Maps a course to another course that dominates it: passing the
/// dominant course exempts the student from this course.
/// One course (CourseId) can have several dominant courses, so this
/// is a self-referencing many-to-many pivot on Course.
/// </summary>
public class DominantCourse
{
    public int Id { get; set; }

    // The "owner" course (oCourse in the API response) that can be
    // exempted.
    public int CourseId { get; set; }

    // The course that, if passed, dominates/exempts it.
    public int DominantCourseId { get; set; }

    public bool IsActive { get; set; } = true;

    public Course Course { get; set; } = null!;

    public Course DominantCourseNav { get; set; } = null!;
}
