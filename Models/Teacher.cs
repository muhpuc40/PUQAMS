namespace PUQAMS.Models;

public class Teacher
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string Fullname { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;
    public bool HasImage { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Varsity { get; set; } = string.Empty;
    public string AccType { get; set; } = "teachers";
    public int Priority { get; set; }
    public string ShortName { get; set; } = string.Empty;

    // Shown in API response ("teachers")
    public string UserRole { get; set; } = "teachers";

    // Used for authorization: Teacher | Moderator | Administrator
    public string SystemRole { get; set; } = "Teacher";

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAtUtc { get; set; }

    public Department Department { get; set; } = null!;
}
