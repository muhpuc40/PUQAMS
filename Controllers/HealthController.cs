using System.Diagnostics;
using System.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUQAMS.Data;

namespace PUQAMS.Controllers;

[ApiController]
[Route("api/v1/Health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public HealthController(
        AppDbContext dbContext,
        IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    // GET /api/v1/Health
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetHealth(
        CancellationToken cancellationToken)
    {
        var healthCheckStopwatch = Stopwatch.StartNew();

        // ---------------------------------------------------------------------
        // Database status
        // ---------------------------------------------------------------------

        var databaseStopwatch = Stopwatch.StartNew();

        var databaseConnected = false;
        string? databaseError = null;

        try
        {
            databaseConnected =
                await _dbContext.Database.CanConnectAsync(cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            databaseConnected = false;

            if (_environment.IsDevelopment())
            {
                databaseError = exception.Message;
            }
        }

        databaseStopwatch.Stop();

        // ---------------------------------------------------------------------
        // API process and memory information
        // ---------------------------------------------------------------------

        using var currentProcess = Process.GetCurrentProcess();

        currentProcess.Refresh();

        var processStartedAtUtc =
            currentProcess.StartTime.ToUniversalTime();

        var processUptime =
            DateTime.UtcNow - processStartedAtUtc;

        var processMemoryBytes =
            currentProcess.WorkingSet64;

        var managedMemoryBytes =
            GC.GetTotalMemory(forceFullCollection: false);

        var gcMemoryInfo =
            GC.GetGCMemoryInfo();

        var totalAvailableMemoryBytes =
            gcMemoryInfo.TotalAvailableMemoryBytes;

        // ---------------------------------------------------------------------
        // Hardware and storage
        // ---------------------------------------------------------------------

        var ramInformation = GetRamHardwareInformation();
        var storageInformation = GetStorageInformation();

        healthCheckStopwatch.Stop();

        var response = new
        {
            success = databaseConnected,

            status = databaseConnected
                ? "Healthy"
                : "Unhealthy",

            message = databaseConnected
                ? "PUQAMS API and database are running successfully."
                : "PUQAMS API is running, but the database is unavailable.",

            api = new
            {
                name = "PUQAMS API",
                version = "1.0",
                environment = _environment.EnvironmentName,
                machineName = Environment.MachineName,
                operatingSystem = Environment.OSVersion.ToString(),
                frameworkVersion = Environment.Version.ToString(),
                processorCount = Environment.ProcessorCount,
                is64BitOperatingSystem =
                    Environment.Is64BitOperatingSystem,
                is64BitProcess =
                    Environment.Is64BitProcess,
                processStartedAtUtc,
                processUptime = FormatDuration(processUptime)
            },

            memory = new
            {
                apiProcessUsedMb =
                    ToMegabytes(processMemoryBytes),

                managedHeapUsedMb =
                    ToMegabytes(managedMemoryBytes),

                totalAvailableToProcessMb =
                    ToMegabytes(totalAvailableMemoryBytes),

                totalAvailableToProcessGb =
                    ToGigabytes(totalAvailableMemoryBytes),

                hardware = ramInformation
            },

            storage = storageInformation,

            database = new
            {
                status = databaseConnected
                    ? "Connected"
                    : "Disconnected",

                provider =
                    _dbContext.Database.ProviderName,

                databaseName =
                    _dbContext.Database
                        .GetDbConnection()
                        .Database,

                server =
                    _dbContext.Database
                        .GetDbConnection()
                        .DataSource,

                responseTimeMs =
                    databaseStopwatch.ElapsedMilliseconds,

                error = databaseError
            },

            healthCheckDurationMs =
                healthCheckStopwatch.ElapsedMilliseconds,

            timestampUtc = DateTime.UtcNow
        };

        if (!databaseConnected)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                response
            );
        }

        return Ok(response);
    }

    // -------------------------------------------------------------------------
    // Storage information
    // -------------------------------------------------------------------------

    private object GetStorageInformation()
    {
        try
        {
            var applicationRootPath =
                Path.GetPathRoot(_environment.ContentRootPath);

            if (string.IsNullOrWhiteSpace(applicationRootPath))
            {
                return new
                {
                    available = false,
                    error = "Unable to determine the application drive."
                };
            }

            var drive = new DriveInfo(applicationRootPath);

            if (!drive.IsReady)
            {
                return new
                {
                    available = false,
                    drive = drive.Name,
                    error = "The application drive is not ready."
                };
            }

            var totalBytes = drive.TotalSize;
            var availableBytes = drive.AvailableFreeSpace;
            var usedBytes = totalBytes - availableBytes;

            var usedPercentage = totalBytes > 0
                ? Math.Round(
                    usedBytes * 100.0 / totalBytes,
                    2
                )
                : 0;

            return new
            {
                available = true,
                drive = drive.Name,
                volumeLabel = drive.VolumeLabel,
                driveType = drive.DriveType.ToString(),
                fileSystem = drive.DriveFormat,

                totalGb =
                    ToGigabytes(totalBytes),

                usedGb =
                    ToGigabytes(usedBytes),

                availableGb =
                    ToGigabytes(availableBytes),

                usedPercentage
            };
        }
        catch (Exception exception)
        {
            return new
            {
                available = false,

                error = _environment.IsDevelopment()
                    ? exception.Message
                    : "Storage information is unavailable."
            };
        }
    }

    // -------------------------------------------------------------------------
    // Physical RAM information for Windows
    // -------------------------------------------------------------------------

    private static object GetRamHardwareInformation()
    {
        if (!OperatingSystem.IsWindows())
        {
            return new
            {
                available = false,
                message =
                    "Physical RAM information is currently supported only on Windows."
            };
        }

        try
        {
            var ramModules = new List<object>();

            ulong totalCapacityBytes = 0;

            using var searcher = new ManagementObjectSearcher(
                "SELECT Capacity, ConfiguredClockSpeed, Speed, " +
                "Manufacturer, PartNumber, DeviceLocator " +
                "FROM Win32_PhysicalMemory"
            );

            using var results = searcher.Get();

            foreach (ManagementObject memoryModule in results)
            {
                var capacityBytes =
                    ConvertToUInt64(memoryModule["Capacity"]);

                var configuredClockSpeed =
                    ConvertToNullableUInt32(
                        memoryModule["ConfiguredClockSpeed"]
                    );

                var hardwareSpeed =
                    ConvertToNullableUInt32(
                        memoryModule["Speed"]
                    );

                totalCapacityBytes += capacityBytes;

                ramModules.Add(new
                {
                    location =
                        memoryModule["DeviceLocator"]?
                            .ToString(),

                    manufacturer =
                        memoryModule["Manufacturer"]?
                            .ToString()?
                            .Trim(),

                    partNumber =
                        memoryModule["PartNumber"]?
                            .ToString()?
                            .Trim(),

                    capacityGb =
                        ToGigabytes((long)capacityBytes),

                    configuredClockSpeedMhz =
                        configuredClockSpeed,

                    hardwareSpeedMhz =
                        hardwareSpeed
                });
            }

            return new
            {
                available = true,

                totalInstalledGb =
                    ToGigabytes((long)totalCapacityBytes),

                moduleCount =
                    ramModules.Count,

                modules =
                    ramModules
            };
        }
        catch (Exception exception)
        {
            return new
            {
                available = false,
                message = exception.Message
            };
        }
    }

    // -------------------------------------------------------------------------
    // Helper methods
    // -------------------------------------------------------------------------

    private static double ToMegabytes(long bytes)
    {
        return Math.Round(
            bytes / 1024d / 1024d,
            2
        );
    }

    private static double ToGigabytes(long bytes)
    {
        return Math.Round(
            bytes / 1024d / 1024d / 1024d,
            2
        );
    }

    private static ulong ConvertToUInt64(object? value)
    {
        return value is null
            ? 0
            : Convert.ToUInt64(value);
    }

    private static uint? ConvertToNullableUInt32(object? value)
    {
        if (value is null)
        {
            return null;
        }

        var convertedValue =
            Convert.ToUInt32(value);

        return convertedValue == 0
            ? null
            : convertedValue;
    }

    private static string FormatDuration(TimeSpan duration)
    {
        return $"{(int)duration.TotalDays}d " +
               $"{duration.Hours}h " +
               $"{duration.Minutes}m " +
               $"{duration.Seconds}s";
    }
}
