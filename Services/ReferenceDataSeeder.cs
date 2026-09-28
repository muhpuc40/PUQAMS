using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;
using PUQAMS.Data.Seed;
using PUQAMS.Models;

namespace PUQAMS.Services;

public sealed class ReferenceDataSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ReferenceDataSeeder> _logger;

    public ReferenceDataSeeder(
        AppDbContext dbContext,
        ILogger<ReferenceDataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Inserts missing records and updates existing records.
    /// Existing records are matched using their Code.
    /// This method does not delete records.
    /// </summary>
    public async Task<ReferenceSeedResult> EnsureSeededAsync(
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

        try
        {
            var result = await SeedCoreAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Deletes all programs and departments and inserts
    /// the predefined data again.
    ///
    /// This should be used only during early development.
    /// </summary>
    public async Task<ReferenceSeedResult> ResetAndSeedAsync(
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

        try
        {
            // Delete child records first.
            await _dbContext.AcademicPrograms.ExecuteDeleteAsync(
                cancellationToken
            );

            // Then delete parent records.
            await _dbContext.Departments.ExecuteDeleteAsync(
                cancellationToken
            );

            _dbContext.ChangeTracker.Clear();

            var result = await SeedCoreAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result with
            {
                WasReset = true
            };
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ReferenceSeedResult> SeedCoreAsync(
        CancellationToken cancellationToken)
    {
        // =============================================================
        // Seed departments
        // =============================================================

        foreach (var seed in ReferenceSeedData.Departments)
        {
            var department =
                await _dbContext.Departments
                    .FirstOrDefaultAsync(
                        x => x.Code == seed.Code,
                        cancellationToken
                    );

            if (department is null)
            {
                department = new Department
                {
                    Code = seed.Code,
                    Name = seed.Name,
                    SortOrder = seed.SortOrder,
                    IsActive = true
                };

                _dbContext.Departments.Add(department);
            }
            else
            {
                department.Name = seed.Name;
                department.SortOrder = seed.SortOrder;
                department.IsActive = true;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        // =============================================================
        // Create department lookup using department code
        // =============================================================

        var departmentRows =
            await _dbContext.Departments
                .ToListAsync(cancellationToken);

        var departmentsByCode =
            departmentRows.ToDictionary(
                x => x.Code,
                StringComparer.OrdinalIgnoreCase
            );

        // =============================================================
        // Seed programs
        // =============================================================

        foreach (var seed in ReferenceSeedData.Programs)
        {
            if (!departmentsByCode.TryGetValue(
                    seed.DepartmentCode,
                    out var department))
            {
                throw new InvalidOperationException(
                    $"Department with code " +
                    $"'{seed.DepartmentCode}' was not found."
                );
            }

            var academicProgram =
                await _dbContext.AcademicPrograms
                    .FirstOrDefaultAsync(
                        x =>
                            x.DepartmentId == department.Id &&
                            x.Code == seed.Code,
                        cancellationToken
                    );

            if (academicProgram is null)
            {
                academicProgram = new AcademicProgram
                {
                    DepartmentId = department.Id,
                    Code = seed.Code,
                    Name = seed.Name,
                    ShortName = seed.ShortName,
                    SortOrder = seed.SortOrder,
                    IsActive = true
                };

                _dbContext.AcademicPrograms.Add(
                    academicProgram
                );
            }
            else
            {
                academicProgram.Name = seed.Name;
                academicProgram.ShortName = seed.ShortName;
                academicProgram.SortOrder = seed.SortOrder;
                academicProgram.IsActive = true;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        // =============================================================
        // Result
        // =============================================================

        var departmentCount =
            await _dbContext.Departments.CountAsync(
                cancellationToken
            );

        var programCount =
            await _dbContext.AcademicPrograms.CountAsync(
                cancellationToken
            );

        _logger.LogInformation(
            "Reference data seeded. Departments: {DepartmentCount}, " +
            "Programs: {ProgramCount}",
            departmentCount,
            programCount
        );

        return new ReferenceSeedResult(
            DepartmentCount: departmentCount,
            ProgramCount: programCount,
            WasReset: false
        );
    }
}

public sealed record ReferenceSeedResult(
    int DepartmentCount,
    int ProgramCount,
    bool WasReset
);