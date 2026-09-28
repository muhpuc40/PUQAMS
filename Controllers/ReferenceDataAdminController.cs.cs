using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PUQAMS.Services;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/v1/admin/reference-data")]
[AllowAnonymous]
public class ReferenceDataAdminController : ControllerBase
{
    private readonly ReferenceDataSeeder _seeder;
    private readonly IWebHostEnvironment _environment;

    public ReferenceDataAdminController(
        ReferenceDataSeeder seeder,
        IWebHostEnvironment environment)
    {
        _seeder = seeder;
        _environment = environment;
    }

    // -----------------------------------------------------------------
    // POST: /api/v1/admin/reference-data/seed
    //
    // Insert missing data and update existing data.
    // It does not delete departments or programs.
    // -----------------------------------------------------------------

    [HttpPost("seed")]
    public async Task<IActionResult> Seed(
        CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        var result =
            await _seeder.EnsureSeededAsync(
                cancellationToken
            );

        return Ok(new
        {
            success = true,
            message =
                "Department and program data seeded successfully.",
            data = result
        });
    }

    // -----------------------------------------------------------------
    // POST:
    // /api/v1/admin/reference-data/refresh?confirm=REFRESH
    //
    // Deletes all programs and departments and inserts them again.
    // -----------------------------------------------------------------

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromQuery] string? confirm,
        CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        if (!string.Equals(
                confirm,
                "REFRESH",
                StringComparison.Ordinal))
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "Pass confirm=REFRESH to refresh reference data."
            });
        }

        var result =
            await _seeder.ResetAndSeedAsync(
                cancellationToken
            );

        return Ok(new
        {
            success = true,
            message =
                "Department and program data refreshed successfully.",
            data = result
        });
    }
}