using System.Text.Json.Serialization;

namespace PUQAMS.Dtos;

public sealed class CourseVersionDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("versionname")] public string VersionName { get; set; } = "";
    [JsonPropertyName("department")] public string Department { get; set; } = "";
    [JsonPropertyName("department_fullname")] public string DepartmentFullName { get; set; } = "";
    [JsonPropertyName("program")] public string Program { get; set; } = "";
    [JsonPropertyName("degreefullname")] public string DegreeFullName { get; set; } = "";
    [JsonPropertyName("program_admitcard")] public string ProgramAdmitCard { get; set; } = "";
}

public sealed class CourseDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("coursecode")] public string CourseCode { get; set; } = "";
    [JsonPropertyName("coursename")] public string CourseName { get; set; } = "";
    [JsonPropertyName("coursecredit")] public double CourseCredit { get; set; }
    [JsonPropertyName("coursetype")] public string CourseType { get; set; } = "";
    [JsonPropertyName("shortname")] public string ShortName { get; set; } = "";
    [JsonPropertyName("ex_semester")] public int ExSemester { get; set; }
    [JsonPropertyName("program")] public string Program { get; set; } = "";

    // Number when set, "" when empty (matches the existing API).
    [JsonPropertyName("course_hr")] public object CourseHr { get; set; } = "";

    [JsonPropertyName("major_name")] public string MajorName { get; set; } = "";
    [JsonPropertyName("transcript_course_code")] public string TranscriptCourseCode { get; set; } = "";
}

public sealed class EquivalentCourseGroupDto
{
    [JsonPropertyName("oCourse")] public CourseDto OCourse { get; set; } = null!;
    [JsonPropertyName("equivalentCourse")] public List<CourseDto> EquivalentCourse { get; set; } = new();
}

public sealed class PrerequisiteCourseGroupDto
{
    [JsonPropertyName("oCourse")] public CourseDto OCourse { get; set; } = null!;
    [JsonPropertyName("prerequisiteCourse")] public List<CourseDto> PrerequisiteCourse { get; set; } = new();
}

public sealed class DominantCourseGroupDto
{
    [JsonPropertyName("oCourse")] public CourseDto OCourse { get; set; } = null!;
    [JsonPropertyName("dominantCourse")] public List<CourseDto> DominantCourse { get; set; } = new();
}

public sealed class PloDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("plo_shortname")] public string PloShortName { get; set; } = "";
    [JsonPropertyName("plo_description")] public string PloDescription { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";

    // 0 or 1 (matches the existing API, which uses a number, not a bool).
    [JsonPropertyName("deleted")] public int Deleted { get; set; }

    [JsonPropertyName("program_id")] public int ProgramId { get; set; }
    [JsonPropertyName("attainment_equation")] public string AttainmentEquation { get; set; } = "";
}

public sealed class CloDto
{
    [JsonPropertyName("course_outcome")] public string CourseOutcome { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";

    // "CSE 1111(V4) - Computer Fundamentals and Ethics (1.5)"
    [JsonPropertyName("course")] public string Course { get; set; } = "";

    [JsonPropertyName("tbl_course_id")] public int TblCourseId { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }

    // Comma-separated PLO short names, e.g. "PLO2,PLO3,PLO5".
    [JsonPropertyName("plos")] public string Plos { get; set; } = "";
}
