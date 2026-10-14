using System.Text.Json.Serialization;

namespace PUQAMS.Dtos;

public sealed class MdRuleNameDto
{
    [JsonPropertyName("rulename")] public string RuleName { get; set; } = "";
    [JsonPropertyName("id")] public int Id { get; set; }
}

public sealed class MdRuleDetailDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("tbl_sessioncourse_column")] public string SessionCourseColumn { get; set; } = "";
    [JsonPropertyName("display_name")] public string DisplayName { get; set; } = "";
    [JsonPropertyName("max_mark")] public int MaxMark { get; set; }

    // "yes" / "no" (matches the existing API, which uses a string here).
    [JsonPropertyName("inRetake")] public string InRetake { get; set; } = "";

    // 0 or 1 (matches the existing API, which uses numbers, not bools).
    [JsonPropertyName("allowA")] public int AllowA { get; set; }
    [JsonPropertyName("allowEXP")] public int AllowExp { get; set; }
    [JsonPropertyName("failifa")] public int FailIfA { get; set; }
    [JsonPropertyName("failifexp")] public int FailIfExp { get; set; }

    [JsonPropertyName("ordering")] public int Ordering { get; set; }
}
