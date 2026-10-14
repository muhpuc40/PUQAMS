namespace PUQAMS.Models;

/// <summary>
/// Maps a course learning outcome to a program learning outcome it
/// addresses. One CLO can map to several PLOs (and one PLO is normally
/// addressed by several CLOs across a program), so this is a
/// many-to-many pivot between Clo and Plo.
/// </summary>
public class CloPlo
{
    public int Id { get; set; }

    public int CloId { get; set; }

    public int PloId { get; set; }

    public Clo Clo { get; set; } = null!;

    public Plo Plo { get; set; } = null!;
}
