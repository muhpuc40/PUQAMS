namespace PUQAMS.Models;

/// <summary>
/// A curriculum version of a program, for example "U-Version-4-CSE".
/// Every version belongs to exactly one program.
/// </summary>
public class CourseVersion
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    // 1, 2, 3, 4 ... unique inside a program
    public int VersionNumber { get; set; }

    // Example: U-Version-4-CSE
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public AcademicProgram Program { get; set; } = null!;

    public ICollection<Course> Courses { get; set; }
        = new List<Course>();
}
