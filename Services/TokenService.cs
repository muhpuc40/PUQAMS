using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PUQAMS.Models;

namespace PUQAMS.Services;

/// <summary>
/// Creates and validates JWT access tokens and JWT refresh tokens.
/// Nothing is stored in the database: a refresh token is a signed JWT
/// that is checked with the same secret key.
///
/// Access token  -> audience = Jwt:Audience
/// Refresh token -> audience = Jwt:Audience + ".refresh"
///
/// Because the audiences differ, a refresh token can never be used as an
/// access token (the JwtBearer middleware rejects it), and an access token
/// can never be used to refresh.
/// </summary>
public sealed class TokenService
{
    private readonly JwtSettings _settings;

    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    private string RefreshAudience => _settings.Audience + ".refresh";

    private SymmetricSecurityKey SigningKey =>
        new(Encoding.UTF8.GetBytes(_settings.Key));

    // -----------------------------------------------------------------
    // Access token (short lived)
    // -----------------------------------------------------------------

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

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_settings.ExpireMinutes),
            signingCredentials: new SigningCredentials(
                SigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    // -----------------------------------------------------------------
    // Refresh token (long lived, stateless)
    // -----------------------------------------------------------------

    public string CreateRefreshToken(Teacher teacher)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, teacher.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: RefreshAudience,
            claims: claims,
            notBefore: now,
            expires: now.AddDays(_settings.RefreshTokenExpireDays),
            signingCredentials: new SigningCredentials(
                SigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Validates a refresh token (signature, issuer, audience, expiry).
    /// Returns the teacher id inside it, or null if it is invalid.
    /// </summary>
    public int? ValidateRefreshToken(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler
        {
            // Keep claim names as written ("sub" stays "sub").
            MapInboundClaims = false
        };

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,

            ValidateAudience = true,
            ValidAudience = RefreshAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningKey,

            // Only accept the algorithm we sign with.
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };

        try
        {
            var principal = handler.ValidateToken(
                refreshToken, parameters, out _);

            var sub = principal.FindFirst(
                JwtRegisteredClaimNames.Sub)?.Value;

            return int.TryParse(sub, out var teacherId)
                ? teacherId
                : null;
        }
        catch (Exception)
        {
            // Bad signature, expired, wrong audience, malformed, ...
            return null;
        }
    }
}
