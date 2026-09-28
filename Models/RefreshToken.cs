namespace PUQAMS.Models;

public class RefreshToken
{
    public long Id { get; set; }

    public int TeacherId { get; set; }

    // SHA-256 hash (hex). The raw token is never stored.
    public string TokenHash { get; set; } = string.Empty;

    public string Device { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }

    public Teacher Teacher { get; set; } = null!;
}