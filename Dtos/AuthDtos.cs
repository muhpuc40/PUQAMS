using System.Text.Json.Serialization;

namespace PUQAMS.Dtos;

public sealed class LoginRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("device")]
    public string? Device { get; set; }

    // Accepts both 1 and "1"
    [JsonPropertyName("department_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int DepartmentId { get; set; }
}

public sealed class RefreshRequest
{
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("device")]
    public string? Device { get; set; }
}

public sealed record TokenResponse(
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);

public sealed record ApiResponse<T>(
    [property: JsonPropertyName("messageCode")] int MessageCode,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("data")] T? Data);

public sealed class AuthUserDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("fullname")] public string Fullname { get; set; } = "";
    [JsonPropertyName("username")] public string Username { get; set; } = "";
    [JsonPropertyName("designation")] public string Designation { get; set; } = "";
    [JsonPropertyName("department")] public string Department { get; set; } = "";
    [JsonPropertyName("tbl_department_id")] public int TblDepartmentId { get; set; }
    [JsonPropertyName("hasimage")] public int HasImage { get; set; }
    [JsonPropertyName("mobile")] public string Mobile { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("address")] public string Address { get; set; } = "";
    [JsonPropertyName("gender")] public string Gender { get; set; } = "";
    [JsonPropertyName("varsity")] public string Varsity { get; set; } = "";
    [JsonPropertyName("acctype")] public string AccType { get; set; } = "";
    [JsonPropertyName("priority")] public int Priority { get; set; }
    [JsonPropertyName("shortname")] public string ShortName { get; set; } = "";
    [JsonPropertyName("user_role")] public string UserRole { get; set; } = "";
}
