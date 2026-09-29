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
        var username = (request.Username ?? string.Empty).Trim();
        var password = request.Password ?? string.Empty;

        if (username.Length == 0 || password.Length == 0)
        {
            return InvalidCredentials();
        }

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
            teacher, teacher.PasswordHash, password);

        if (verify == PasswordVerificationResult.Failed)
        {
            return InvalidCredentials();
        }

        if (verify == PasswordVerificationResult.SuccessRehashNeeded)
        {
            teacher.PasswordHash =
                _passwordHasher.HashPassword(teacher, password);
        }

        teacher.LastLoginAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new TokenResponse(
            _tokenService.CreateAccessToken(teacher),
            _tokenService.CreateRefreshToken(teacher)));
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
    // Body: { "refresh_token": "..." }
    // Returns a new access token and a new refresh token.
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var teacherId =
            _tokenService.ValidateRefreshToken(request.RefreshToken);

        if (teacherId is null)
        {
            return InvalidRefreshToken();
        }

        // Load the teacher so the new access token has fresh role and
        // department data, and so deactivated users cannot refresh.
        var teacher = await _dbContext.Teachers
            .AsNoTracking()
            .Include(x => x.Department)
            .FirstOrDefaultAsync(
                x => x.Id == teacherId.Value,
                cancellationToken);

        if (teacher is null ||
            !teacher.IsActive ||
            !teacher.Department.IsActive)
        {
            return InvalidRefreshToken();
        }

        return Ok(new TokenResponse(
            _tokenService.CreateAccessToken(teacher),
            _tokenService.CreateRefreshToken(teacher)));
    }

    // -----------------------------------------------------------------

    private IActionResult InvalidCredentials()
        => Unauthorized(new ApiResponse<object>(
            1401, "Invalid department, username or password.", null));

    private IActionResult InvalidRefreshToken()
        => Unauthorized(new ApiResponse<object>(
            1401, "Invalid or expired refresh token.", null));
}
