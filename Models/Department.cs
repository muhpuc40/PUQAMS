namespace PUQAMS.Models;

public class Department
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<AcademicProgram> Programs { get; set; }
        = new List<AcademicProgram>();
}
