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

    public DbSet<CourseType> CourseTypes => Set<CourseType>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<EquivalentCourse> EquivalentCourses =>
        Set<EquivalentCourse>();

    public DbSet<PrerequisiteCourse> PrerequisiteCourses =>
        Set<PrerequisiteCourse>();

    public DbSet<DominantCourse> DominantCourses =>
        Set<DominantCourse>();

    public DbSet<Plo> Plos => Set<Plo>();

    public DbSet<Clo> Clos => Set<Clo>();

    public DbSet<CloPlo> CloPlos => Set<CloPlo>();

    public DbSet<MdRuleName> MdRuleNames => Set<MdRuleName>();

    public DbSet<MdRuleDetail> MdRuleDetails => Set<MdRuleDetail>();

    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<SessionCategory> SessionCategories => Set<SessionCategory>();

    public DbSet<ActualSession> ActualSessions => Set<ActualSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDepartment(modelBuilder);
        ConfigureAcademicProgram(modelBuilder);
        ConfigureTeacher(modelBuilder);
        ConfigureCourseType(modelBuilder);
        ConfigureCourseVersion(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureEquivalentCourse(modelBuilder);
        ConfigurePrerequisiteCourse(modelBuilder);
        ConfigureDominantCourse(modelBuilder);
        ConfigurePlo(modelBuilder);
        ConfigureClo(modelBuilder);
        ConfigureCloPlo(modelBuilder);
        ConfigureMdRuleName(modelBuilder);
        ConfigureMdRuleDetail(modelBuilder);
        ConfigureSessionCategory(modelBuilder);
        ConfigureActualSession(modelBuilder);
        ConfigureSession(modelBuilder);
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

    private static void ConfigureCourseType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.ToTable("course_types");

            entity.HasKey(x => x.Id);

            // Ids are seeded explicitly (1 Theory, 2 Lab, ...), so the
            // column stays a normal int rather than an identity column.
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.Name).HasMaxLength(30).IsRequired();

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            entity.HasIndex(x => x.Name).IsUnique();
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
            entity.Property(x => x.ShortName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MajorName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TranscriptCourseCode).HasMaxLength(50).IsRequired();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // One course code per version.
            entity.HasIndex(x => new { x.VersionId, x.CourseCode })
                .IsUnique();

            entity.HasIndex(x => x.CourseTypeId);

            entity.HasOne(x => x.Version)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.VersionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CourseType)
                .WithMany()
                .HasForeignKey(x => x.CourseTypeId)
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

    private static void ConfigurePlo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plo>(entity =>
        {
            entity.ToTable("plos");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.ShortName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
            entity.Property(x => x.AttainmentEquation).HasMaxLength(500).IsRequired();

            entity.Property(x => x.IsDeleted).HasDefaultValue(false);

            // One short name per program (PLO1 of CSE is distinct from
            // PLO1 of EEE, but a program cannot define PLO1 twice).
            entity.HasIndex(x => new { x.ProgramId, x.ShortName })
                .IsUnique();

            entity.HasOne(x => x.Program)
                .WithMany()
                .HasForeignKey(x => x.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureClo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clo>(entity =>
        {
            entity.ToTable("clos");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.CourseOutcome).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000).IsRequired();

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            // One course outcome label per course (CO1 of a course
            // cannot be defined twice).
            entity.HasIndex(x => new { x.CourseId, x.CourseOutcome })
                .IsUnique();

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCloPlo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CloPlo>(entity =>
        {
            entity.ToTable("clo_plos");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // A CLO cannot map to the same PLO twice.
            entity.HasIndex(x => new { x.CloId, x.PloId })
                .IsUnique();

            entity.HasOne(x => x.Clo)
                .WithMany(x => x.CloPlos)
                .HasForeignKey(x => x.CloId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Plo)
                .WithMany(x => x.CloPlos)
                .HasForeignKey(x => x.PloId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMdRuleName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MdRuleName>(entity =>
        {
            entity.ToTable("md_rulenames");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.RuleName).HasMaxLength(150).IsRequired();

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            // A department cannot define the same rule name twice for
            // the same course type.
            entity.HasIndex(x => new { x.DepartmentId, x.CourseTypeId, x.RuleName })
                .IsUnique();

            entity.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CourseType)
                .WithMany()
                .HasForeignKey(x => x.CourseTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMdRuleDetail(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MdRuleDetail>(entity =>
        {
            entity.ToTable("md_rule_details");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.SessionCourseColumn).HasMaxLength(100).IsRequired();
            entity.Property(x => x.DisplayName).HasMaxLength(150).IsRequired();

            entity.Property(x => x.InRetake).HasDefaultValue(true);
            entity.Property(x => x.AllowA).HasDefaultValue(true);
            entity.Property(x => x.AllowExp).HasDefaultValue(true);
            entity.Property(x => x.FailIfA).HasDefaultValue(false);
            entity.Property(x => x.FailIfExp).HasDefaultValue(true);

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            // A rule cannot list the same session-course column twice.
            entity.HasIndex(x => new { x.RuleId, x.SessionCourseColumn })
                .IsUnique();

            entity.HasOne(x => x.Rule)
                .WithMany(x => x.Details)
                .HasForeignKey(x => x.RuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureSession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("sessions");

            entity.HasKey(x => x.Id);

            // Ids are preserved from the source system (sessions are
            // referenced by id elsewhere), so no identity column.
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.SessionName).HasMaxLength(250).IsRequired();
            entity.Property(x => x.ExamName).HasMaxLength(250).IsRequired();
            entity.Property(x => x.ExamHeldIn).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();

            entity.Property(x => x.IsEnrollmentOpen).HasDefaultValue(false);
            entity.Property(x => x.IsEnrollmentOpenStu).HasDefaultValue(false);
            entity.Property(x => x.IsRoutinePublished).HasDefaultValue(false);
            entity.Property(x => x.IsCreditTransfer).HasDefaultValue(false);
            entity.Property(x => x.IsActive).HasDefaultValue(true);

            entity.HasIndex(x => new { x.ProgramId, x.Status });
            entity.HasIndex(x => x.SessionCategoryId);
            entity.HasIndex(x => x.ActualSessionId);

            entity.HasOne(x => x.Program)
                .WithMany()
                .HasForeignKey(x => x.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.SessionCategory)
                .WithMany()
                .HasForeignKey(x => x.SessionCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ActualSession)
                .WithMany()
                .HasForeignKey(x => x.ActualSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSessionCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionCategory>(entity =>
        {
            entity.ToTable("session_categories");

            entity.HasKey(x => x.Id);

            // Ids are seeded explicitly (1 Semester System, 2 Trimester
            // System, ...), so the column stays a normal int.
            entity.Property(x => x.Id).ValueGeneratedNever();

            entity.Property(x => x.Name).HasMaxLength(50).IsRequired();

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            entity.HasIndex(x => x.Name).IsUnique();
        });
    }

    private static void ConfigureActualSession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActualSession>(entity =>
        {
            entity.ToTable("actual_sessions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.SessionName).HasMaxLength(100).IsRequired();

            entity.Property(x => x.IsActive).HasDefaultValue(true);

            entity.HasIndex(x => x.SessionName).IsUnique();
        });
    }
}
