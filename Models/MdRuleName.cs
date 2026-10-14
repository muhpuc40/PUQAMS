namespace PUQAMS.Models;

/// <summary>
/// A named mark-distribution rule (e.g. "CSE-MD-Theory-1"), scoped to a
/// department and a course type. Its line items live in MdRuleDetail.
/// </summary>
public class MdRuleName
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int CourseTypeId { get; set; }

    public string RuleName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Department Department { get; set; } = null!;

    public CourseType CourseType { get; set; } = null!;

    public ICollection<MdRuleDetail> Details { get; set; }
        = new List<MdRuleDetail>();
}
