namespace PUQAMS.Models;

/// <summary>
/// The kind of a session: Semester System, Trimester System,
/// Project/Thesis, Credit Transfer, Certification. Referenced by
/// Session.SessionCategoryId.
/// </summary>
public class SessionCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
