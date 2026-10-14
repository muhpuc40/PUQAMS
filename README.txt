PUQAMS - setup (no C# seeder; reference data is seeded via raw SQL)

Requirements: .NET 8 SDK, MySQL on 127.0.0.1:3306 (root, empty password - see appsettings.json)

1. Install tools and packages
   dotnet tool install --global dotnet-ef --version 8.*   (only once)
   dotnet restore

2. Create/update the database schema
   This copy already has an InitialCreate migration (departments, programs,
   teachers, course_versions, courses, equivalent_courses,
   prerequisite_courses, dominant_courses). It does NOT yet have the new
   plos, clos and clo_plos tables, so add one more migration for those:

     dotnet ef migrations add AddPlosAndClos
     dotnet ef database update

   (If you are starting from an empty Migrations folder instead, just run
   `dotnet ef migrations add InitialCreate` once - it will include every
   table, PLOs/CLOs included - then `dotnet ef database update`.)

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
   owner+related course pair / program+PLO short name / course+CLO
   outcome label / CLO+PLO pair), so running it again updates existing
   rows instead of duplicating them. It never overwrites a teacher's
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
   GET  /api/Course/get_versionwise_plos?version_id=<V4 id>&program_id=1
   GET  /api/CourseManagement/get_clos_by_course?course_id=<CSE 1111(V4) id>&program_id=1

   Use the V4 version's own id from get_programwise_course_version, and
   the course's own id from get_versionwise_course_list - ids are
   auto-generated, so they won't match any legacy system's ids.

PLOs (plos table) belong to a program, not a specific curriculum version,
so get_versionwise_plos uses version_id only to confirm it belongs to
program_id; every version of that program shares the same PLO list.

CLOs (clos table) belong to one course. A CLO can map to more than one
PLO through the clo_plos pivot table; get_clos_by_course returns that as
a comma-separated "plos" field (e.g. "PLO2,PLO3,PLO5").

To add more equivalent/prerequisite/dominant relations, PLOs, CLOs or
CLO-PLO mappings later, add more INSERT ... ON DUPLICATE KEY UPDATE
blocks to seed.sql following the same pattern, and re-run the file.
