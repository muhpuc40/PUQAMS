namespace PUQAMS.Models;

/// <summary>
/// The kind of a course: Theory, Lab, Project, MS-Theory, MS-Seminar, ...
/// Referenced by Course.CourseTypeId and by MdRuleName.CourseTypeId.
/// </summary>
public class CourseType
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
