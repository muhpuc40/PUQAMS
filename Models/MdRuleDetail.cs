namespace PUQAMS.Models;

/// <summary>
/// One mark-distribution line item of a MdRuleName (e.g. "Attendance",
/// worth 10 marks, counted in retake).
/// </summary>
public class MdRuleDetail
{
    public int Id { get; set; }

    public int RuleId { get; set; }

    // Example: Attendance, Assignment, Class_Test, Mid_Term, Final
    public string SessionCourseColumn { get; set; } = string.Empty;

    // Example: Class Attendance, Home Works, Mid Term, Final Exam
    public string DisplayName { get; set; } = string.Empty;

    public int MaxMark { get; set; }

    // Whether this component counts again when the course is retaken.
    public bool InRetake { get; set; } = true;

    public bool AllowA { get; set; } = true;
    public bool AllowExp { get; set; } = true;
    public bool FailIfA { get; set; }
    public bool FailIfExp { get; set; } = true;

    // Display order within the rule.
    public int Ordering { get; set; }

    public bool IsActive { get; set; } = true;

    public MdRuleName Rule { get; set; } = null!;
}
