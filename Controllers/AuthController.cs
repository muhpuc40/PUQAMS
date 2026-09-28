using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;
using PUQAMS.Dtos;
using PUQAMS.Models;
using PUQAMS.Services;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly TokenService _tokenService;
    private readonly IPasswordHasher<Teacher> _passwordHasher;

    public AuthController(
        AppDbContext dbContext,
        TokenService tokenService,
        IPasswordHasher<Teacher> passwordHasher)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    // POST: /api/Auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();

        var teacher = await _dbContext.Teachers
            .FirstOrDefaultAsync(x =>
                x.Username == username &&
                x.DepartmentId == request.DepartmentId &&
                x.IsActive &&
                x.Department.IsActive,
                cancellationToken);

        // Same message for every failure, so usernames cannot be probed.
        if (teacher is null)
        {
            return InvalidCredentials();
        }

        var verify = _passwordHasher.VerifyHashedPassword(
            teacher, teacher.PasswordHash, request.Password);

        if (verify == PasswordVerificationResult.Failed)
        {
            return InvalidCredentials();
        }

        if (verify == PasswordVerificationResult.SuccessRehashNeeded)
        {
            teacher.PasswordHash =
                _passwordHasher.HashPassword(teacher, request.Password);
        }

        teacher.LastLoginAtUtc = DateTime.UtcNow;

        var response = await IssueTokensAsync(
            teacher, request.Device, cancellationToken);

        return Ok(response);
    }

    // GET: /api/Auth/get_auth
    [Authorize]
    [HttpGet("get_auth")]
    public async Task<IActionResult> GetAuth(
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var teacherId))
        {
            return Unauthorized(new ApiResponse<object>(
                1401, "Invalid token.", null));
        }

        var user = await _dbContext.Teachers
            .AsNoTracking()
            .Where(x => x.Id == teacherId && x.IsActive)
            .Select(x => new AuthUserDto
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Username = x.Username,
                Designation = x.Designation,
                Department = x.Department.Name,
                TblDepartmentId = x.DepartmentId,
                HasImage = x.HasImage ? 1 : 0,
                Mobile = x.Mobile,
                Email = x.Email,
                Address = x.Address,
                Gender = x.Gender,
                Varsity = x.Varsity,
                AccType = x.AccType,
                Priority = x.Priority,
                ShortName = x.ShortName,
                UserRole = x.UserRole
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Unauthorized(new ApiResponse<object>(
                1401, "User not found or inactive.", null));
        }

        return Ok(new ApiResponse<AuthUserDto>(1200, "Success", user));
    }

    // POST: /api/Auth/refresh
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var hash = TokenService.Hash(request.RefreshToken);
        var now = DateTime.UtcNow;

        var stored = await _dbContext.RefreshTokens
            .Include(x => x.Teacher)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);

        if (stored is null)
        {
            return InvalidRefreshToken();
        }

        // A revoked token being reused means it may have been stolen:
        // revoke every active session of that teacher.
        if (stored.RevokedAtUtc is not null)
        {
            await _dbContext.RefreshTokens
                .Where(x => x.TeacherId == stored.TeacherId &&
                            x.RevokedAtUtc == null)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(x => x.RevokedAtUtc, now),
                    cancellationToken);

            return InvalidRefreshToken();
        }

        if (stored.ExpiresAtUtc <= now ||
            !stored.Teacher.IsActive ||
            !stored.Teacher.Department.IsActive)
        {
            return InvalidRefreshToken();
        }

        // Rotate: revoke the old token and issue a new pair.
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newHash = TokenService.Hash(newRefreshToken);

        stored.RevokedAtUtc = now;
        stored.ReplacedByTokenHash = newHash;

        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            TeacherId = stored.TeacherId,
            TokenHash = newHash,
            Device = request.Device ?? stored.Device,
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(_tokenService.RefreshTokenDays)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new TokenResponse(
            _tokenService.CreateAccessToken(stored.Teacher),
            newRefreshToken));
    }

    // POST: /api/Auth/logout
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var hash = TokenService.Hash(request.RefreshToken);

        await _dbContext.RefreshTokens
            .Where(x => x.TokenHash == hash && x.RevokedAtUtc == null)
            .ExecuteUpdateAsync(
                s => s.SetProperty(x => x.RevokedAtUtc, DateTime.UtcNow),
                cancellationToken);

        return Ok(new ApiResponse<object>(1200, "Success", null));
    }

    // -----------------------------------------------------------------

    private async Task<TokenResponse> IssueTokensAsync(
        Teacher teacher,
        string? device,
        CancellationToken cancellationToken)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        var now = DateTime.UtcNow;

        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            TeacherId = teacher.Id,
            TokenHash = TokenService.Hash(refreshToken),
            Device = string.IsNullOrWhiteSpace(device)
                ? "Unknown"
                : device.Trim(),
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(_tokenService.RefreshTokenDays)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse(
            _tokenService.CreateAccessToken(teacher),
            refreshToken);
    }

    private IActionResult InvalidCredentials()
        => Unauthorized(new ApiResponse<object>(
            1401, "Invalid department, username or password.", null));

    private IActionResult InvalidRefreshToken()
        => Unauthorized(new ApiResponse<object>(
            1401, "Invalid or expired refresh token.", null));
}