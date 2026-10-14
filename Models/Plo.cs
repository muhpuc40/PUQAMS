namespace PUQAMS.Models;

/// <summary>
/// A Program Learning Outcome. Defined per program (not per curriculum
/// version), so a PLO applies to every version of its program.
/// </summary>
public class Plo
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    // Example: PLO1
    public string ShortName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Active | Inactive
    public string Status { get; set; } = "Active";

    public bool IsDeleted { get; set; }

    // Example: "[ (0.20 x PLO1.i) + (0.20 x PLO1.ii) ]". Often empty.
    public string AttainmentEquation { get; set; } = string.Empty;

    public AcademicProgram Program { get; set; } = null!;

    public ICollection<CloPlo> CloPlos { get; set; } = new List<CloPlo>();
}
