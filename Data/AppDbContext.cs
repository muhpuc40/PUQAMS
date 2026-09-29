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

    public DbSet<Teacher> Teachers => Set<Teacher>();

    public DbSet<CourseVersion> CourseVersions =>
        Set<CourseVersion>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<EquivalentCourse> EquivalentCourses =>
        Set<EquivalentCourse>();

    public DbSet<PrerequisiteCourse> PrerequisiteCourses =>
        Set<PrerequisiteCourse>();

    public DbSet<DominantCourse> DominantCourses =>
        Set<DominantCourse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDepartment(modelBuilder);
        ConfigureAcademicProgram(modelBuilder);
        ConfigureTeacher(modelBuilder);
        ConfigureCourseVersion(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureEquivalentCourse(modelBuilder);
        ConfigurePrerequisiteCourse(modelBuilder);
        ConfigureDominantCourse(modelBuilder);
    }

    private static void ConfigureDepartment(ModelBuilder modelBuilder)
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

    private static void ConfigureAcademicProgram(ModelBuilder modelBuilder)
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

            entity.Property(x => x.Level)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.SortOrder)
                .HasDefaultValue(0);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(x => new { x.DepartmentId, x.Code })
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

    private static void ConfigureCourseVersion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseVersion>(entity =>
        {
            entity.ToTable("course_versions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // One version number per program.
            entity.HasIndex(x => new { x.ProgramId, x.VersionNumber })
                .IsUnique();

            entity.HasOne(x => x.Program)
                .WithMany(x => x.Versions)
                .HasForeignKey(x => x.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.CourseCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CourseName).HasMaxLength(250).IsRequired();
            entity.Property(x => x.CourseCredit).HasPrecision(4, 1);
            entity.Property(x => x.CourseType).HasMaxLength(30).IsRequired();
            entity.Property(x => x.ShortName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MajorName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TranscriptCourseCode).HasMaxLength(50).IsRequired();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // One course code per version.
            entity.HasIndex(x => new { x.VersionId, x.CourseCode })
                .IsUnique();

            entity.HasOne(x => x.Version)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.VersionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // Three self-referencing many-to-many pivots on Course: one course
    // can have several equivalent / prerequisite / dominant courses.
    // Both foreign keys point at Course, so OnDelete is Restrict on
    // both sides to avoid multiple cascade paths.

    private static void ConfigureEquivalentCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquivalentCourse>(entity =>
        {
            entity.ToTable("equivalent_courses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // A course cannot list the same equivalent course twice.
            entity.HasIndex(x => new { x.CourseId, x.EquivalentCourseId })
                .IsUnique();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.EquivalentCourseNav)
                .WithMany()
                .HasForeignKey(x => x.EquivalentCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePrerequisiteCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrerequisiteCourse>(entity =>
        {
            entity.ToTable("prerequisite_courses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(x => new { x.CourseId, x.PrerequisiteCourseId })
                .IsUnique();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PrerequisiteCourseNav)
                .WithMany()
                .HasForeignKey(x => x.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureDominantCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DominantCourse>(entity =>
        {
            entity.ToTable("dominant_courses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasIndex(x => new { x.CourseId, x.DominantCourseId })
                .IsUnique();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.DominantCourseNav)
                .WithMany()
                .HasForeignKey(x => x.DominantCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
