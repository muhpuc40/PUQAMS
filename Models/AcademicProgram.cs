namespace PUQAMS.Models;

public class AcademicProgram
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public Department Department { get; set; } = null!;
}