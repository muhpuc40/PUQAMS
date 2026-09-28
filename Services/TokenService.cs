using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PUQAMS.Models;

namespace PUQAMS.Services;

public sealed class TokenService
{
    private readonly JwtSettings _settings;

    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public int RefreshTokenDays => _settings.RefreshTokenExpireDays;

    public string CreateAccessToken(Teacher teacher)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, teacher.Id.ToString()),
            new(ClaimTypes.Name, teacher.Username),
            new(ClaimTypes.Role, teacher.SystemRole),
            new("department_id", teacher.DepartmentId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Key));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_settings.ExpireMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public static string Hash(string token)
        => Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}