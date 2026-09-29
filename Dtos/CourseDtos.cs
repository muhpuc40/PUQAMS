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
