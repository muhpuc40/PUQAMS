namespace PUQAMS.Models;

/// <summary>
/// The canonical session/term names (e.g. "Spring 2025", "Fall 2025")
/// that a program-wise Session is an instance of. Referenced by
/// Session.ActualSessionId.
/// </summary>
public class ActualSession
{
    public int Id { get; set; }

    public string SessionName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
