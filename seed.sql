-- =====================================================================
-- PUQAMS reference data seed script (MySQL)
--
-- Run this AFTER `dotnet ef database update` has created the tables.
-- Safe to run more than once: every insert upserts by its natural key,
-- so re-running this file updates existing rows instead of duplicating
-- them.
--
-- Default teacher password for every seeded account: Premier123456
-- Change it after first login. The PasswordHash values below are real
-- ASP.NET Core Identity PBKDF2-SHA256 hashes (100,000 iterations),
-- generated for that password, so login works immediately.
-- =====================================================================

START TRANSACTION;

-- ---------------------------------------------------------------------
-- Departments
-- ---------------------------------------------------------------------

INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('CSE', 'Computer Science & Engineering', 1, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('EEE', 'Electrical & Electronic Engineering', 2, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('ARCH', 'Architecture', 3, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('DELL', 'English Language & Literature', 4, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('DLAW', 'Law', 5, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('DECO', 'Economics', 6, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('DMATH', 'Mathematics', 7, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('FBS', 'Business Administration', 8, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('SSD', 'Sociology and Sustainable Development', 9, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('PH', 'Public Health', 10, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('LIS', 'Library and Information Science', 11, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('FDT', 'Fashion Design and Technology', 12, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('UTS-IT', 'Premier UTS Information Technology', 13, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('UTS-BUSINESS', 'Premier UTS Business', 14, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO departments (Code, Name, SortOrder, IsActive)
VALUES ('DIPLOMA-PLUS', 'Premier UTS Diploma Plus', 15, 1)
ON DUPLICATE KEY UPDATE Name=VALUES(Name), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Programs (one per department, several for CSE/FBS/etc.)
-- ---------------------------------------------------------------------

INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BSC-CSE', 'Bachelor of Science (Engineering) in Computer Science and Engineering', 'B.Sc. in CSE', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MSC-CSE', 'Master of Science in Computer Science and Engineering', 'MSc in CSE', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BSC-EEE', 'Bachelor of Science in Electrical and Electronic Engineering', 'BSc in EEE', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'EEE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BARCH', 'Bachelor of Architecture', 'B.Arch', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'ARCH'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BA-ENGLISH', 'Bachelor of Arts Honours in English', 'BA (Hons.) in English', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'DELL'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MA-ENGLISH', 'Master of Arts in English', 'MA in English', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'DELL'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'LLB-HONS', 'Bachelor of Laws Honours', 'LL.B. (Hons.)', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'DLAW'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'LLM', 'Master of Laws', 'LL.M.', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'DLAW'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BSS-ECONOMICS', 'Bachelor of Social Science Honours in Economics', 'BSS (Hons.) in Economics', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'DECO'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MSS-ECONOMICS', 'Master of Social Science in Economics', 'MSS in Economics', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'DECO'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BSC-MATH', 'Bachelor of Science Honours in Mathematics', 'BSc (Hons.) in Mathematics', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'DMATH'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MSC-MATH', 'Master of Science in Mathematics', 'MSc in Mathematics', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'DMATH'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA', 'Bachelor of Business Administration', 'BBA', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA-MKT', 'Bachelor of Business Administration in Marketing', 'BBA (MKT)', 'Undergraduate', 2, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA-MGT', 'Bachelor of Business Administration in Management', 'BBA (MGT)', 'Undergraduate', 3, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA-ACC', 'Bachelor of Business Administration in Accounting', 'BBA (ACC)', 'Undergraduate', 4, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA-HRM', 'Bachelor of Business Administration in Human Resource Management', 'BBA (HRM)', 'Undergraduate', 5, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BBA-FIN', 'Bachelor of Business Administration in Finance', 'BBA (FIN)', 'Undergraduate', 6, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MBA-1Y', 'Master of Business Administration One Year', 'MBA (1-Year)', 'Postgraduate', 7, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MBA-1-5Y', 'Master of Business Administration One and Half Year', 'MBA (1.5-Year)', 'Postgraduate', 8, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MBA-2Y', 'Master of Business Administration Two Year', 'MBA (2-Year)', 'Postgraduate', 9, 1
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BSS-SSD', 'Bachelor of Social Science Honours in Sociology and Sustainable Development', 'BSS (Hons.) in SSD', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'SSD'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MSS-SSD', 'Master of Social Science in Sociology and Sustainable Development', 'MSS in SSD', 'Postgraduate', 2, 1
FROM departments d WHERE d.Code = 'SSD'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'MPH', 'Master of Public Health', 'MPH', 'Postgraduate', 1, 1
FROM departments d WHERE d.Code = 'PH'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'PGD-LIS', 'Postgraduate Diploma in Library and Information Science', 'PGD in LIS', 'Postgraduate', 1, 1
FROM departments d WHERE d.Code = 'LIS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'BA-FDT', 'Bachelor of Arts in Fashion Design and Technology', 'BA in FDT', 'Undergraduate', 1, 1
FROM departments d WHERE d.Code = 'FDT'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'DIPLOMA-IT', 'Diploma of Information Technology', 'Diploma of IT', 'Diploma', 1, 1
FROM departments d WHERE d.Code = 'UTS-IT'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'DIPLOMA-BUSINESS', 'Diploma of Business', 'Diploma of Business', 'Diploma', 1, 1
FROM departments d WHERE d.Code = 'UTS-BUSINESS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);
INSERT INTO programs (DepartmentId, Code, Name, ShortName, Level, SortOrder, IsActive)
SELECT d.Id, 'DIPLOMA-PLUS', 'Diploma Plus', 'Diploma Plus', 'Diploma', 1, 1
FROM departments d WHERE d.Code = 'DIPLOMA-PLUS'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), ShortName=VALUES(ShortName), Level=VALUES(Level), SortOrder=VALUES(SortOrder), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Teachers
-- Password for all of them: Premier123456
-- NOTE: the ON DUPLICATE KEY branch does NOT touch PasswordHash, so
-- re-running this script never resets a password that was changed later.
-- ---------------------------------------------------------------------

INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (CSE)', 'admin_cse', 'AQAAAAEAAYagAAAAEDy87xMMALrkr3rM46g/+kn4cZP2tLHAu+t4LNb59X8BEO0UEncykreBALkhj5wV/Q==', 'Lecturer', 0,
       '01675304383', 'alokchy04@yahoo.com', 'PUC', 'Male', '', 'teachers', 26, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'CSE'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Moderator (CSE)', 'moderator_cse', 'AQAAAAEAAYagAAAAELicBcwnERGzzs/RrHpRzuuRBFrfAyplWy4ZKmYMIZZP5Ys7MdHOfysMMvb2yGUgvQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 27, '',
       'teachers', 'Moderator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'CSE'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Teacher (CSE)', 'teacher_cse', 'AQAAAAEAAYagAAAAECyLt7thC6Xucu8L6OQDnwr6D2z+JV882F49vkIrnRKH+vzp3yEtNnJSqQFZWrERLQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 28, '',
       'teachers', 'Teacher', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'CSE'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (EEE)', 'admin_eee', 'AQAAAAEAAYagAAAAEP6zPki0o62Ij+7q6nayZseTaEzVDve/kw+ZM/NE/ZX2CXXi4Tbg4JA6egOwRbqFdQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'EEE'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (ARCH)', 'admin_arch', 'AQAAAAEAAYagAAAAEFOat4Y+2cLjFW6hL0H0jpN/WyneI6l5QjuTjBygIIPnX1aQVyGll7/XAOyej4allA==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'ARCH'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (DELL)', 'admin_dell', 'AQAAAAEAAYagAAAAEFrjhn5ZQRoUxFHvtAXV6Q+zp6fWdy359Id7QJNr8mzTBxjYz3YIfDw+zjaYEIu5qg==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'DELL'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (Law)', 'admin_dlaw', 'AQAAAAEAAYagAAAAEKVngw1xpM1lNrMET433Oo0hEDDyukzMHn5FWOGWjsDwhcsJ67GuKeVGdvmF960qTw==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'DLAW'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (Economics)', 'admin_deco', 'AQAAAAEAAYagAAAAEMvKGEAi2SZ0BWtGz3Q7KpCdfe9/at9iSGqS/bhWQljPoZMmPHW4tS/RGQzGNn6B6Q==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'DECO'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (Mathematics)', 'admin_dmath', 'AQAAAAEAAYagAAAAEOLeyjJtE+stHJwZh7OtnetIY61DWr/ws1Rmw4aFLH1GEuAlhCMekM2/TzbB9rtoZQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'DMATH'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (FBS)', 'admin_fbs', 'AQAAAAEAAYagAAAAEDrpSEcqzqHsFvqStaVhRCuVAnE2kkpkEXcBNO8CEb0P+K6aEiajigere8DGd5LmcQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'FBS'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (SSD)', 'admin_ssd', 'AQAAAAEAAYagAAAAEFpej8OHcIzqKqqHs/1fbQwtEhcHDbaW1Gn8eu6CWGwf8U+nt4br+grPPPCuCwtWjA==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'SSD'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (Public Health)', 'admin_ph', 'AQAAAAEAAYagAAAAEDRCAB262/tWpifW7zoLVKSiGHBodNoJa5tg0NohabAhrvTuu0I6Cy54qhiv0ujFxQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'PH'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (LIS)', 'admin_lis', 'AQAAAAEAAYagAAAAECdT8EXrozwI4pRWBQUlf5U2ccpK1ODiTVzriEU5wfEY/pGt/enY1/sr9+aKSKZVIQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'LIS'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (FDT)', 'admin_fdt', 'AQAAAAEAAYagAAAAEMGJnJYWaMKc704vXYM26eHchcYKjiBbJZ5b4BMHLhfGJCpA3eqzo5ohJ9vG2mDS5Q==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'FDT'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (UTS IT)', 'admin_uts_it', 'AQAAAAEAAYagAAAAEGlaG6OKX3pmuVhgisi3B89/f3pdu5WabL6gHypmoEc5BXS6Q5BxSb6CB7ndy0/O5g==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'UTS-IT'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (UTS Business)', 'admin_uts_business', 'AQAAAAEAAYagAAAAEKaYGyvCDsyWqRkBQFt1fNyPUdjVZDAYHHblojF0LPknGppe04tyJdl0Pi3SeFoA2g==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'UTS-BUSINESS'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);
INSERT INTO teachers
    (DepartmentId, Fullname, Username, PasswordHash, Designation, HasImage,
     Mobile, Email, Address, Gender, Varsity, AccType, Priority, ShortName,
     UserRole, SystemRole, IsActive, CreatedAtUtc)
SELECT d.Id, 'Administrator (Diploma Plus)', 'admin_diploma_plus', 'AQAAAAEAAYagAAAAEBag6OTVraHzAXiceWxrb6Sq+auL8UlYq1b/SwBY7VhQIAPZxhO+lMJBv+i5k9hEgQ==', 'Lecturer', 0,
       '', '', 'PUC', 'Male', '', 'teachers', 0, '',
       'teachers', 'Administrator', 1, UTC_TIMESTAMP()
FROM departments d WHERE d.Code = 'DIPLOMA-PLUS'
ON DUPLICATE KEY UPDATE
    DepartmentId=VALUES(DepartmentId), Fullname=VALUES(Fullname),
    Designation=VALUES(Designation), Mobile=VALUES(Mobile), Email=VALUES(Email),
    Address=VALUES(Address), Gender=VALUES(Gender), Priority=VALUES(Priority),
    SystemRole=VALUES(SystemRole), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Course versions (program wise)
-- ---------------------------------------------------------------------

INSERT INTO course_versions (ProgramId, VersionNumber, Name, IsActive)
SELECT p.Id, 1, 'U-Version-1-CSE', 1
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), IsActive=VALUES(IsActive);
INSERT INTO course_versions (ProgramId, VersionNumber, Name, IsActive)
SELECT p.Id, 2, 'U-Version-2-CSE', 1
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), IsActive=VALUES(IsActive);
INSERT INTO course_versions (ProgramId, VersionNumber, Name, IsActive)
SELECT p.Id, 3, 'U-Version-3-CSE', 1
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), IsActive=VALUES(IsActive);
INSERT INTO course_versions (ProgramId, VersionNumber, Name, IsActive)
SELECT p.Id, 4, 'U-Version-4-CSE', 1
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Name=VALUES(Name), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Courses
-- Version 4 = current curriculum (BSc in CSE, semester 1).
-- Versions 2 and 3 hold only the handful of legacy courses needed
-- below to demonstrate equivalent courses across versions.
-- ---------------------------------------------------------------------

INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'BAN 1101(V4)', 'Functional Bengali Language', 2, 'Theory',
       'FBL', 1, NULL, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1111(V4)', 'Computer Fundamentals and Ethics', 1.5, 'Theory',
       'CFE', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1113(V4)', 'Programming Fundamentals', 3, 'Theory',
       'PF', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1114(V4)', 'Programming Fundamentals Laboratory', 1.5, 'Lab',
       'PFL', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'EEE 1101(V4)', 'Introduction to Electrical Engineering', 3, 'Theory',
       'IEE', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'EEE 1102(V4)', 'Introduction to Electrical Engineering Laboratory', 1.5, 'Lab',
       'IEEL', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'ENG 1101(V4)', 'General English', 3, 'Theory',
       'GE', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'MAT 1203(V4)', 'Differential Integral Calculus', 3, 'Theory',
       'DIC', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 4
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1113(V3)', 'Structured Programming', 3, 'Theory',
       'SP', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 3
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1114(V3)', 'Structured Programming Laboratory', 1.5, 'Lab',
       'SPL', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 3
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'EEE 1101(V3)', 'Basic Electrical Engineering', 3, 'Theory',
       'BEE', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 3
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);
INSERT INTO courses
    (VersionId, CourseCode, CourseName, CourseCredit, CourseType,
     ShortName, ExSemester, CourseHour, MajorName, TranscriptCourseCode, IsActive)
SELECT cv.Id, 'CSE 1113(V2)', 'Introduction to Programming', 3, 'Theory',
       'IP', 1, 3, '', '', 1
FROM course_versions cv
JOIN programs p ON p.Id = cv.ProgramId
JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE' AND cv.VersionNumber = 2
ON DUPLICATE KEY UPDATE
    CourseName=VALUES(CourseName), CourseCredit=VALUES(CourseCredit),
    CourseType=VALUES(CourseType), ShortName=VALUES(ShortName),
    ExSemester=VALUES(ExSemester), CourseHour=VALUES(CourseHour), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Equivalent courses
-- One course can map to more than one equivalent course (e.g. across curriculum versions).
-- ---------------------------------------------------------------------

INSERT INTO equivalent_courses (CourseId, EquivalentCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1113(V4)'
  AND tc.CourseCode = 'CSE 1113(V3)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO equivalent_courses (CourseId, EquivalentCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1113(V4)'
  AND tc.CourseCode = 'CSE 1113(V2)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO equivalent_courses (CourseId, EquivalentCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1114(V4)'
  AND tc.CourseCode = 'CSE 1114(V3)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO equivalent_courses (CourseId, EquivalentCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'EEE 1101(V4)'
  AND tc.CourseCode = 'EEE 1101(V3)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Prerequisite courses
-- One course can require more than one prerequisite course.
-- ---------------------------------------------------------------------

INSERT INTO prerequisite_courses (CourseId, PrerequisiteCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1114(V4)'
  AND tc.CourseCode = 'CSE 1113(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO prerequisite_courses (CourseId, PrerequisiteCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'EEE 1102(V4)'
  AND tc.CourseCode = 'EEE 1101(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO prerequisite_courses (CourseId, PrerequisiteCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1113(V4)'
  AND tc.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO prerequisite_courses (CourseId, PrerequisiteCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1113(V4)'
  AND tc.CourseCode = 'MAT 1203(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);

-- ---------------------------------------------------------------------
-- Dominant courses
-- One course can be exempted by more than one dominant course.
-- ---------------------------------------------------------------------

INSERT INTO dominant_courses (CourseId, DominantCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'CSE 1111(V4)'
  AND tc.CourseCode = 'CSE 1113(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO dominant_courses (CourseId, DominantCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'BAN 1101(V4)'
  AND tc.CourseCode = 'ENG 1101(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO dominant_courses (CourseId, DominantCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'EEE 1102(V4)'
  AND tc.CourseCode = 'EEE 1101(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);
INSERT INTO dominant_courses (CourseId, DominantCourseId, IsActive)
SELECT oc.Id, tc.Id, 1
FROM courses oc, courses tc
WHERE oc.CourseCode = 'EEE 1102(V4)'
  AND tc.CourseCode = 'CSE 1114(V4)'
ON DUPLICATE KEY UPDATE IsActive = VALUES(IsActive);

-- ---------------------------------------------------------------------
-- PLOs (Program Learning Outcomes)
-- Defined per program, shared by every version of that program.
-- Seeded here for BSc in CSE (department CSE, program BSC-CSE).
-- ---------------------------------------------------------------------

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO1', 'Engineering knowledge', 'Active', 0,
       '[ (0.20 x PLO1.i) + (0.20 x PLO1.ii) + (0.3 x PLO1.iii) + (0.3 x PLO1.iv)]'
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO2', 'Problem analysis', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO3', 'Design/development of solutions', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO4', 'Conduct investigations of complex problems', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO5', 'Modern tool usage', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO6', 'The engineer and society', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO7', 'Environment and sustainability', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO8', 'Ethics', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO9', 'Individual work and teamwork', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO10', 'Communication', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO11', 'Project management and finance', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

INSERT INTO plos (ProgramId, ShortName, Description, Status, IsDeleted, AttainmentEquation)
SELECT p.Id, 'PLO12', 'Life-long learning', 'Active', 0, ''
FROM programs p JOIN departments d ON d.Id = p.DepartmentId
WHERE d.Code = 'CSE' AND p.Code = 'BSC-CSE'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), Status=VALUES(Status),
    IsDeleted=VALUES(IsDeleted), AttainmentEquation=VALUES(AttainmentEquation);

-- ---------------------------------------------------------------------
-- CLOs (Course Learning Outcomes)
-- Seeded for CSE 1111(V4) - Computer Fundamentals and Ethics.
-- ---------------------------------------------------------------------

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO1', 'Identify the fundamental elements of a computer system.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO2', 'List the basic use of computing and internet technologies.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO3', 'Practice word processing, spreadsheet, and presentation application for performing basic tasks.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO4', 'Classify number systems and their applications in computing.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO5', 'Define computer ethics and privacy issues of computing and the cyber world.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO6', 'Write an individual or group report to disseminate the experiment execution process and findings.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

INSERT INTO clos (CourseId, CourseOutcome, Description, IsActive)
SELECT c.Id, 'CO7', 'Apply problem-solving and modern tool usage to basic computing tasks.', 1
FROM courses c WHERE c.CourseCode = 'CSE 1111(V4)'
ON DUPLICATE KEY UPDATE Description=VALUES(Description), IsActive=VALUES(IsActive);

-- ---------------------------------------------------------------------
-- CLO -> PLO mappings
-- One CLO can map to more than one PLO (CO7 maps to three).
-- ---------------------------------------------------------------------

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO1'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO1'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO2'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO1'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO3'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO2'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO4'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO2'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO5'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO8'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO6'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName = 'PLO4'
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);

INSERT INTO clo_plos (CloId, PloId)
SELECT cl.Id, pl.Id
FROM clos cl
JOIN courses c ON c.Id = cl.CourseId
JOIN plos pl JOIN programs pr ON pr.Id = pl.ProgramId JOIN departments d ON d.Id = pr.DepartmentId
WHERE c.CourseCode = 'CSE 1111(V4)' AND cl.CourseOutcome = 'CO7'
  AND d.Code = 'CSE' AND pr.Code = 'BSC-CSE' AND pl.ShortName IN ('PLO2','PLO3','PLO5')
ON DUPLICATE KEY UPDATE CloId = VALUES(CloId);


COMMIT;
