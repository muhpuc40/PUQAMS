namespace PUQAMS.Data.Seed;

public sealed record DepartmentSeed(
    string Code,
    string Name,
    int SortOrder
);

public sealed record ProgramSeed(
    string DepartmentCode,
    string Code,
    string Name,
    string ShortName,
    int SortOrder
);
public sealed record TeacherSeed(
    string Username,
    string DepartmentCode,
    string Fullname,
    string Designation = "Lecturer",
    string Mobile = "",
    string Email = "",
    string Address = "PUC",
    string Gender = "Male",
    int Priority = 0,
    string SystemRole = "Administrator"
);

public static class ReferenceSeedData
{
    // Development-only default password. Change it after first login.
    public const string DefaultPassword = "Premier123456";

    public static readonly IReadOnlyList<TeacherSeed> Teachers =
    [
        new("admin_cse", "CSE", "Administrator (CSE)",
        Mobile: "01675304383", Email: "alokchy04@yahoo.com", Priority: 26),
    new("moderator_cse", "CSE", "Moderator (CSE)",
        SystemRole: "Moderator", Priority: 27),
    new("teacher_cse", "CSE", "Teacher (CSE)",
        SystemRole: "Teacher", Priority: 28),

    new("admin_eee", "EEE", "Administrator (EEE)"),
    new("admin_arch", "ARCH", "Administrator (ARCH)"),
    new("admin_dell", "DELL", "Administrator (DELL)"),
    new("admin_dlaw", "DLAW", "Administrator (Law)"),
    new("admin_deco", "DECO", "Administrator (Economics)"),
    new("admin_dmath", "DMATH", "Administrator (Mathematics)"),
    new("admin_fbs", "FBS", "Administrator (FBS)"),
    new("admin_ssd", "SSD", "Administrator (SSD)"),
    new("admin_ph", "PH", "Administrator (Public Health)"),
    new("admin_lis", "LIS", "Administrator (LIS)"),
    new("admin_fdt", "FDT", "Administrator (FDT)"),
    new("admin_uts_it", "UTS-IT", "Administrator (UTS IT)"),
    new("admin_uts_business", "UTS-BUSINESS", "Administrator (UTS Business)"),
    new("admin_diploma_plus", "DIPLOMA-PLUS", "Administrator (Diploma Plus)")
    ];


    public static readonly IReadOnlyList<DepartmentSeed> Departments =
    [
        new(
            "CSE",
            "Computer Science & Engineering",
            1
        ),

        new(
            "EEE",
            "Electrical & Electronic Engineering",
            2
        ),

        new(
            "ARCH",
            "Architecture",
            3
        ),

        new(
            "DELL",
            "English Language & Literature",
            4
        ),

        new(
            "DLAW",
            "Law",
            5
        ),

        new(
            "DECO",
            "Economics",
            6
        ),

        new(
            "DMATH",
            "Mathematics",
            7
        ),

        new(
            "FBS",
            "Business Administration",
            8
        ),

        new(
            "SSD",
            "Sociology and Sustainable Development",
            9
        ),

        new(
            "PH",
            "Public Health",
            10
        ),

        new(
            "LIS",
            "Library and Information Science",
            11
        ),

        new(
            "FDT",
            "Fashion Design and Technology",
            12
        ),

        new(
            "UTS-IT",
            "Premier UTS Information Technology",
            13
        ),

        new(
            "UTS-BUSINESS",
            "Premier UTS Business",
            14
        ),

        new(
            "DIPLOMA-PLUS",
            "Premier UTS Diploma Plus",
            15
        )
    ];

    public static readonly IReadOnlyList<ProgramSeed> Programs =
    [
        // ============================================================
        // Computer Science and Engineering
        // ============================================================

        new(
            "CSE",
            "BSC-CSE",
            "Bachelor of Science in Computer Science and Engineering",
            "BSc in CSE",
            1
        ),

        new(
            "CSE",
            "MSC-CSE",
            "Master of Science in Computer Science and Engineering",
            "MSc in CSE",
            2
        ),

        // ============================================================
        // Electrical and Electronic Engineering
        // ============================================================

        new(
            "EEE",
            "BSC-EEE",
            "Bachelor of Science in Electrical and Electronic Engineering",
            "BSc in EEE",
            1
        ),

        // ============================================================
        // Architecture
        // ============================================================

        new(
            "ARCH",
            "BARCH",
            "Bachelor of Architecture",
            "B.Arch",
            1
        ),

        // ============================================================
        // English Language and Literature
        // ============================================================

        new(
            "DELL",
            "BA-ENGLISH",
            "Bachelor of Arts Honours in English",
            "BA (Hons.) in English",
            1
        ),

        new(
            "DELL",
            "MA-ENGLISH",
            "Master of Arts in English",
            "MA in English",
            2
        ),

        // ============================================================
        // Law
        // ============================================================

        new(
            "DLAW",
            "LLB-HONS",
            "Bachelor of Laws Honours",
            "LL.B. (Hons.)",
            1
        ),

        new(
            "DLAW",
            "LLM",
            "Master of Laws",
            "LL.M.",
            2
        ),

        // ============================================================
        // Economics
        // ============================================================

        new(
            "DECO",
            "BSS-ECONOMICS",
            "Bachelor of Social Science Honours in Economics",
            "BSS (Hons.) in Economics",
            1
        ),

        new(
            "DECO",
            "MSS-ECONOMICS",
            "Master of Social Science in Economics",
            "MSS in Economics",
            2
        ),

        // ============================================================
        // Mathematics
        // ============================================================

        new(
            "DMATH",
            "BSC-MATH",
            "Bachelor of Science Honours in Mathematics",
            "BSc (Hons.) in Mathematics",
            1
        ),

        new(
            "DMATH",
            "MSC-MATH",
            "Master of Science in Mathematics",
            "MSc in Mathematics",
            2
        ),

        // ============================================================
        // Business Administration
        // ============================================================

        new(
            "FBS",
            "BBA",
            "Bachelor of Business Administration",
            "BBA",
            1
        ),

        new(
            "FBS",
            "BBA-MKT",
            "Bachelor of Business Administration in Marketing",
            "BBA (MKT)",
            2
        ),

        new(
            "FBS",
            "BBA-MGT",
            "Bachelor of Business Administration in Management",
            "BBA (MGT)",
            3
        ),

        new(
            "FBS",
            "BBA-ACC",
            "Bachelor of Business Administration in Accounting",
            "BBA (ACC)",
            4
        ),

        new(
            "FBS",
            "BBA-HRM",
            "Bachelor of Business Administration in Human Resource Management",
            "BBA (HRM)",
            5
        ),

        new(
            "FBS",
            "BBA-FIN",
            "Bachelor of Business Administration in Finance",
            "BBA (FIN)",
            6
        ),

        new(
            "FBS",
            "MBA-1Y",
            "Master of Business Administration One Year",
            "MBA (1-Year)",
            7
        ),

        new(
            "FBS",
            "MBA-1-5Y",
            "Master of Business Administration One and Half Year",
            "MBA (1.5-Year)",
            8
        ),

        new(
            "FBS",
            "MBA-2Y",
            "Master of Business Administration Two Year",
            "MBA (2-Year)",
            9
        ),

        // ============================================================
        // Sociology and Sustainable Development
        // ============================================================

        new(
            "SSD",
            "BSS-SSD",
            "Bachelor of Social Science Honours in Sociology and Sustainable Development",
            "BSS (Hons.) in SSD",
            1
        ),

        new(
            "SSD",
            "MSS-SSD",
            "Master of Social Science in Sociology and Sustainable Development",
            "MSS in SSD",
            2
        ),

        // ============================================================
        // Public Health
        // ============================================================

        new(
            "PH",
            "MPH",
            "Master of Public Health",
            "MPH",
            1
        ),

        // ============================================================
        // Library and Information Science
        // ============================================================

        new(
            "LIS",
            "PGD-LIS",
            "Postgraduate Diploma in Library and Information Science",
            "PGD in LIS",
            1
        ),

        // ============================================================
        // Fashion Design and Technology
        // ============================================================

        new(
            "FDT",
            "BA-FDT",
            "Bachelor of Arts in Fashion Design and Technology",
            "BA in FDT",
            1
        ),

        // ============================================================
        // Premier UTS Information Technology
        // ============================================================

        new(
            "UTS-IT",
            "DIPLOMA-IT",
            "Diploma of Information Technology",
            "Diploma of IT",
            1
        ),

        // ============================================================
        // Premier UTS Business
        // ============================================================

        new(
            "UTS-BUSINESS",
            "DIPLOMA-BUSINESS",
            "Diploma of Business",
            "Diploma of Business",
            1
        ),

        // ============================================================
        // Diploma Plus
        // ============================================================

        new(
            "DIPLOMA-PLUS",
            "DIPLOMA-PLUS",
            "Diploma Plus",
            "Diploma Plus",
            1
        )
    ];
}