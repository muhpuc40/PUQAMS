using System.Text.Json.Serialization;

namespace PUQAMS.Dtos;

public sealed class SessionCategoryDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
}

public sealed class ActualSessionDto
{
    [JsonPropertyName("session_name")] public string SessionName { get; set; } = "";
}

public sealed class SessionDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("sessionname")] public string SessionName { get; set; } = "";
    [JsonPropertyName("startdate")] public string StartDate { get; set; } = "";

    // "" when the session has no end date yet, else a date string
    // (matches the existing API, which never sends a JSON null here).
    [JsonPropertyName("enddate")] public string EndDate { get; set; } = "";

    [JsonPropertyName("isenrollmentopen")] public int IsEnrollmentOpen { get; set; }
    [JsonPropertyName("isenrollmentopen_stu")] public int IsEnrollmentOpenStu { get; set; }
    [JsonPropertyName("examname")] public string ExamName { get; set; } = "";
    [JsonPropertyName("exam_held_in")] public string ExamHeldIn { get; set; } = "";
    [JsonPropertyName("isroutinepublished")] public int IsRoutinePublished { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";

    // "" when false, 1 when true (matches the existing API, which leaves
    // this field blank rather than sending 0).
    [JsonPropertyName("iscredit_transfer")] public object IsCreditTransfer { get; set; } = "";
}
