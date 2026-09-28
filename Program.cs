using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PUQAMS.Data;
using PUQAMS.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// DATABASE
// =============================================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is not configured."
    );

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// =============================================================================
// JWT SETTINGS
// =============================================================================

var jwtKey =
    builder.Configuration["Jwt:Key"];

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT signing key 'Jwt:Key' is not configured."
    );
}

if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new InvalidOperationException(
        "JWT issuer 'Jwt:Issuer' is not configured."
    );
}

if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "JWT audience 'Jwt:Audience' is not configured."
    );
}

// =============================================================================
// AUTHENTICATION
// =============================================================================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });

// =============================================================================
// AUTHORIZATION
// =============================================================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "TeacherOnly",
        policy => policy.RequireRole("Teacher")
    );

    options.AddPolicy(
        "ModeratorOnly",
        policy => policy.RequireRole("Moderator")
    );

    options.AddPolicy(
        "AdministratorOnly",
        policy => policy.RequireRole("Administrator")
    );

    options.AddPolicy(
        "ModeratorOrAdministrator",
        policy => policy.RequireRole(
            "Moderator",
            "Administrator"
        )
    );
});

// =============================================================================
// CONTROLLERS AND JSON
// =============================================================================

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );

        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// =============================================================================
// APPLICATION SERVICES
// =============================================================================

builder.Services.AddScoped<ReferenceDataSeeder>();

// =============================================================================
// SWAGGER
// =============================================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "PUQAMS API",
            Version = "v1",
            Description =
                "Premier University Question Authoring and Moderation System API"
        }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Description =
                "Enter the JWT access token.",

            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type =
                            ReferenceType.SecurityScheme,

                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

// =============================================================================
// CORS
// =============================================================================

var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? new[]
    {
        "http://localhost:3000",
        "http://localhost:5173"
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "FrontendPolicy",
        policy =>
        {
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

// =============================================================================
// BUILD APPLICATION
// =============================================================================

var app = builder.Build();

// =============================================================================
// DEVELOPMENT MIGRATION AND SEEDING
// =============================================================================

if (app.Environment.IsDevelopment())
{
    await using var scope =
        app.Services.CreateAsyncScope();

    var serviceProvider =
        scope.ServiceProvider;

    var logger =
        serviceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("ApplicationStartup");

    try
    {
        var dbContext =
            serviceProvider
                .GetRequiredService<AppDbContext>();

        logger.LogInformation(
            "Applying database migrations."
        );

        await dbContext.Database.MigrateAsync();

        logger.LogInformation(
            "Database migrations completed."
        );

        var seeder =
            serviceProvider
                .GetRequiredService<ReferenceDataSeeder>();

        logger.LogInformation(
            "Starting department and program seeding."
        );

        var seedResult =
            await seeder.EnsureSeededAsync();

        logger.LogInformation(
            "Reference data seeded. Departments: {DepartmentCount}, " +
            "Programs: {ProgramCount}",
            seedResult.DepartmentCount,
            seedResult.ProgramCount
        );
    }
    catch (Exception exception)
    {
        logger.LogError(
            exception,
            "Database migration or seeding failed."
        );

        // The server remains running so the error can be inspected
        // through the console and health endpoint.
    }
}

// =============================================================================
// SWAGGER MIDDLEWARE
// =============================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "PUQAMS API v1"
        );

        options.DocumentTitle =
            "PUQAMS API";
    });
}

// =============================================================================
// HTTP PIPELINE
// =============================================================================

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// =============================================================================
// RUN
// =============================================================================

await app.RunAsync();