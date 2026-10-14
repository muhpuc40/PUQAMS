namespace PUQAMS.Models;

/// <summary>
/// An exam/enrollment session (term) of a program, e.g. "Fall 2027 (B. Sc.)".
/// Ids are preserved from the source system (not auto-generated) since
/// sessions are referenced by id elsewhere.
/// </summary>
public class Session
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    public int SessionCategoryId { get; set; }

    public int ActualSessionId { get; set; }

    public string SessionName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    // Null when the session has no end date yet.
    public DateTime? EndDate { get; set; }

    public bool IsEnrollmentOpen { get; set; }

    public bool IsEnrollmentOpenStu { get; set; }

    public string ExamName { get; set; } = string.Empty;

    public string ExamHeldIn { get; set; } = string.Empty;

    public bool IsRoutinePublished { get; set; }

    // "Running" | "Completed"
    public string Status { get; set; } = "Running";

    public bool IsCreditTransfer { get; set; }

    public bool IsActive { get; set; } = true;

    public AcademicProgram Program { get; set; } = null!;

    public SessionCategory SessionCategory { get; set; } = null!;

    public ActualSession ActualSession { get; set; } = null!;
}
