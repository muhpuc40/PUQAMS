using Microsoft.EntityFrameworkCore;
using PUQAMS.Models;

namespace PUQAMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Teacher> Teachers => Set<Teacher>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Department> Departments =>
        Set<Department>();

    public DbSet<AcademicProgram> AcademicPrograms =>
        Set<AcademicProgram>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDepartment(modelBuilder);
        ConfigureAcademicProgram(modelBuilder);
        ConfigureTeacher(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }

    private static void ConfigureDepartment(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Code)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.SortOrder)
                .HasDefaultValue(0);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });
    }

    private static void ConfigureAcademicProgram(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicProgram>(entity =>
        {
            entity.ToTable("programs");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(x => x.ShortName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.SortOrder)
                .HasDefaultValue(0);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(x => new
            {
                x.DepartmentId,
                x.Code
            })
            .IsUnique();

            entity.HasOne(x => x.Department)
                .WithMany(x => x.Programs)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTeacher(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("teachers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Fullname).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();

            entity.Property(x => x.Designation).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Mobile).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Gender).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Varsity).HasMaxLength(150).IsRequired();
            entity.Property(x => x.AccType).HasMaxLength(30).IsRequired();
            entity.Property(x => x.ShortName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.UserRole).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SystemRole).HasMaxLength(30).IsRequired();

            entity.HasIndex(x => x.Username).IsUnique();
            entity.HasIndex(x => x.DepartmentId);

            entity.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.Device).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ReplacedByTokenHash).HasMaxLength(128);

            entity.HasIndex(x => x.TokenHash).IsUnique();

            entity.HasOne(x => x.Teacher)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}