using Microsoft.EntityFrameworkCore;
using PUQAMS.Models;

namespace PUQAMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments =>
        Set<Department>();

    public DbSet<AcademicProgram> AcademicPrograms =>
        Set<AcademicProgram>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDepartment(modelBuilder);
        ConfigureAcademicProgram(modelBuilder);
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
}