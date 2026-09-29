PUQAMS - setup (no C# seeder; reference data is seeded via raw SQL)

Requirements: .NET 8 SDK, MySQL on 127.0.0.1:3306 (root, empty password - see appsettings.json)

1. Install tools and packages
   dotnet tool install --global dotnet-ef --version 8.*   (only once)
   dotnet restore

2. Create/update the database schema
   If you don't have a Migrations folder yet, generate one fresh migration
   against the current model (Department, AcademicProgram, Teacher,
   CourseVersion, Course, EquivalentCourse, PrerequisiteCourse,
   DominantCourse):

     dotnet ef migrations add InitialCreate
     dotnet ef database update

   If you already have an InitialCreate migration (departments, programs,
   teachers, course_versions, courses) and are only adding the three new
   relation tables, add a second migration instead:

     dotnet ef migrations add AddCourseRelationTables
     dotnet ef database update

   Startup also runs `dbContext.Database.MigrateAsync()` automatically in
   Development, so any migration you add is applied on next run.

3. Seed reference data
   No C# seeder runs anymore. Load the data with the included seed.sql,
   using any MySQL client:

     mysql -h 127.0.0.1 -u root puqams < seed.sql

   Or open seed.sql in MySQL Workbench / DBeaver / HeidiSQL and execute it
   against the puqams database.

   seed.sql is idempotent: every insert upserts by its natural key
   (Code / Username / program+version number / version+course code /
   owner+related course pair), so running it again updates existing rows
   instead of duplicating them. It never overwrites a teacher's
   PasswordHash on re-run.

   Default password for every seeded teacher: Premier123456

4. Run
   dotnet run --launch-profile https

Test (all need Authorization: Bearer <token> except login and the
departments list):
   GET  /api/v1/departments
   POST /api/Auth/login   { "username": "admin_cse", "password": "Premier123456",
                             "device": "Application", "department_id": "1" }
   GET  /api/Auth/get_auth
   GET  /api/Course/get_programwise_course_version?program_id=1
   GET  /api/Course/get_versionwise_course_list?version_id=<V4 id>&program_id=1
   GET  /api/Course/get_equivalent_courses?version_id=<V4 id>&program_id=1
   GET  /api/Course/get_prerequisite_courses?version_id=<V4 id>&program_id=1
   GET  /api/Course/get_dominant_courses?version_id=<V4 id>&program_id=1

   Use the V4 version's own id from get_programwise_course_version - ids
   are auto-generated, so they won't match any legacy system's ids.

To add more equivalent/prerequisite/dominant relations later, add more
INSERT ... ON DUPLICATE KEY UPDATE blocks to seed.sql following the same
pattern (owner CourseCode -> related CourseCode), and re-run the file.
