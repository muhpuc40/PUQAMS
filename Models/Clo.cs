namespace PUQAMS.Models;

/// <summary>
/// A Course Learning Outcome, defined on one course. One CLO can map to
/// several PLOs, through the CloPlo pivot (CloPlos).
/// </summary>
public class Clo
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    // Example: CO1, CLO10
    public string CourseOutcome { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Course Course { get; set; } = null!;

    public ICollection<CloPlo> CloPlos { get; set; } = new List<CloPlo>();
}
