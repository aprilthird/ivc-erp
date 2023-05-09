using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PaternalSurname = table.Column<string>(nullable: false),
                    MaternalSurname = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    DocumentType = table.Column<int>(nullable: false),
                    Document = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteQualityCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SamplingDate = table.Column<DateTime>(nullable: false),
                    TestDate = table.Column<DateTime>(nullable: false),
                    CertificateSerialNumber = table.Column<string>(nullable: false),
                    For07SerialNumber = table.Column<string>(nullable: false),
                    CertificateFileUrl = table.Column<string>(nullable: true),
                    For07FileUrl = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    FirstResult = table.Column<double>(nullable: false),
                    SecondResult = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteQualityCertificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foremen",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PaternalSurname = table.Column<string>(nullable: false),
                    MaternalSurname = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    DocumentType = table.Column<int>(nullable: false),
                    Document = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foremen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssuerTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuerTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Letters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ReferenceId = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DocumentType = table.Column<int>(nullable: false),
                    ResponseTerm = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Letters_Letters_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentTypeId = table.Column<Guid>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Propietary = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LetterInterestGroups",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(nullable: false),
                    InterestGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterInterestGroups", x => new { x.LetterId, x.InterestGroupId });
                    table.ForeignKey(
                        name: "FK_LetterInterestGroups_InterestGroup_InterestGroupId",
                        column: x => x.InterestGroupId,
                        principalTable: "InterestGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LetterInterestGroups_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LetterIssuerTargets",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(nullable: false),
                    IssuerTargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterIssuerTargets", x => new { x.LetterId, x.IssuerTargetId });
                    table.ForeignKey(
                        name: "FK_LetterIssuerTargets_IssuerTargets_IssuerTargetId",
                        column: x => x.IssuerTargetId,
                        principalTable: "IssuerTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LetterIssuerTargets_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificateIssuers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateIssuers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateIssuers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBudgetCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBudgetCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBudgetCategories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualificationZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualificationZones_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quarries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quarries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemPhases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FillingLaboratoryTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: false),
                    RecordNumber = table.Column<string>(nullable: false),
                    TestDate = table.Column<DateTime>(nullable: false),
                    SamplingDate = table.Column<DateTime>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    OriginType = table.Column<int>(nullable: false),
                    CertificateIssuerId = table.Column<Guid>(nullable: false),
                    Ubication = table.Column<string>(nullable: false),
                    MaterialMoisture = table.Column<double>(nullable: false),
                    OptimumMoisture = table.Column<double>(nullable: false),
                    MaxDensity = table.Column<double>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillingLaboratoryTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FillingLaboratoryTests_CertificateIssuers_CertificateIssuerId",
                        column: x => x.CertificateIssuerId,
                        principalTable: "CertificateIssuers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBudgedItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    MeasurementUnit = table.Column<string>(nullable: true),
                    Measure = table.Column<float>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    ProjectBudgetCategoryId = table.Column<Guid>(nullable: true),
                    ProjectBudgetItemParent = table.Column<Guid>(nullable: true),
                    ProjectBudgedItemParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBudgedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBudgedItems_ProjectBudgedItems_ProjectBudgedItemParentId",
                        column: x => x.ProjectBudgedItemParentId,
                        principalTable: "ProjectBudgedItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectBudgedItems_ProjectBudgetCategories_ProjectBudgetCategoryId",
                        column: x => x.ProjectBudgetCategoryId,
                        principalTable: "ProjectBudgetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFronts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SystemPhaseId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Class = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFronts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFronts_SystemPhases_SystemPhaseId",
                        column: x => x.SystemPhaseId,
                        principalTable: "SystemPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFronts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    PaternalSurname = table.Column<string>(nullable: false),
                    MaternalSurname = table.Column<string>(nullable: false),
                    DocumentType = table.Column<int>(nullable: false),
                    Document = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    WorkArea = table.Column<int>(nullable: false),
                    EntryPosition = table.Column<string>(nullable: true),
                    CurrentPosition = table.Column<string>(nullable: true),
                    WorkFrontHeadId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_WorkFronts_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    ForemanId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroups_Foremen_ForemanId",
                        column: x => x.ForemanId,
                        principalTable: "Foremen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerGroups_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionDailyParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionDailyParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionDailyParts_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DrainageArea = table.Column<string>(nullable: true),
                    TerrainType = table.Column<int>(nullable: false),
                    InternalDiameter = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Thickness = table.Column<double>(nullable: false),
                    Cover = table.Column<double>(nullable: false),
                    Bottom = table.Column<double>(nullable: false),
                    InputOutput = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerBoxes_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteQualityCertificateDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConcreteQualityCertificateId = table.Column<Guid>(nullable: false),
                    Segment = table.Column<int>(nullable: false),
                    SegmentNumber = table.Column<int>(nullable: false),
                    SewerBoxId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteQualityCertificateDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcreteQualityCertificateDetails_ConcreteQualityCertificates_ConcreteQualityCertificateId",
                        column: x => x.ConcreteQualityCertificateId,
                        principalTable: "ConcreteQualityCertificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcreteQualityCertificateDetails_SewerBoxes_SewerBoxId",
                        column: x => x.SewerBoxId,
                        principalTable: "SewerBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    QualificationZoneId = table.Column<Guid>(nullable: false),
                    InitialSewerBoxId = table.Column<Guid>(nullable: false),
                    FinalSewerBoxId = table.Column<Guid>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    DrainageArea = table.Column<string>(nullable: true),
                    AverageDepthSewerBox = table.Column<double>(nullable: false),
                    AverageDepthSewerLine = table.Column<double>(nullable: false),
                    TiltedPipelineLengthOnAxis = table.Column<double>(nullable: false),
                    HorizontalDistanceOnAxis = table.Column<double>(nullable: false),
                    InstalledPipelineLength = table.Column<double>(nullable: false),
                    ExcavationLength = table.Column<double>(nullable: false),
                    Slope = table.Column<double>(nullable: false),
                    NominalDiameter = table.Column<double>(nullable: false),
                    PipelineType = table.Column<int>(nullable: false),
                    PipelineClass = table.Column<int>(nullable: false),
                    Piping = table.Column<double>(nullable: false),
                    TerrainType = table.Column<int>(nullable: false),
                    HasFor47 = table.Column<bool>(nullable: false),
                    ExcavationLengthPercentForNormal = table.Column<double>(nullable: false),
                    ExcavationLengthPercentForRocky = table.Column<double>(nullable: false),
                    ExcavationLengthPercentForSemirocous = table.Column<double>(nullable: false),
                    ExcavationLengthForNormal = table.Column<double>(nullable: false),
                    ExcavationLengthForRocky = table.Column<double>(nullable: false),
                    ExcavationLengthForSemirocous = table.Column<double>(nullable: false),
                    AddedLately = table.Column<bool>(nullable: false),
                    IsReviewed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerLines_SewerBoxes_FinalSewerBoxId",
                        column: x => x.FinalSewerBoxId,
                        principalTable: "SewerBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerLines_SewerBoxes_InitialSewerBoxId",
                        column: x => x.InitialSewerBoxId,
                        principalTable: "SewerBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerLines_QualificationZones_QualificationZoneId",
                        column: x => x.QualificationZoneId,
                        principalTable: "QualificationZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerLines_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompactionDensityCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: false),
                    ExecutionDate = table.Column<DateTime>(nullable: false),
                    SewerLineId = table.Column<Guid>(nullable: false),
                    MaterialType = table.Column<int>(nullable: false),
                    QuarryId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompactionDensityCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompactionDensityCertificates_Quarries_QuarryId",
                        column: x => x.QuarryId,
                        principalTable: "Quarries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompactionDensityCertificates_SewerLines_SewerLineId",
                        column: x => x.SewerLineId,
                        principalTable: "SewerLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompactionDensityCertificateDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TestDate = table.Column<DateTime>(nullable: false),
                    CompactionDensityCertificateId = table.Column<Guid>(nullable: false),
                    FillingLaboratoryTestId = table.Column<Guid>(nullable: false),
                    WetDensity = table.Column<double>(nullable: false),
                    Moisture = table.Column<double>(nullable: false),
                    DryDensity = table.Column<double>(nullable: false),
                    DensityPercentage = table.Column<double>(nullable: false),
                    Layer = table.Column<int>(nullable: false),
                    Latest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompactionDensityCertificateDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompactionDensityCertificateDetails_CompactionDensityCertificates_CompactionDensityCertificateId",
                        column: x => x.CompactionDensityCertificateId,
                        principalTable: "CompactionDensityCertificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompactionDensityCertificateDetails_FillingLaboratoryTests_FillingLaboratoryTestId",
                        column: x => x.FillingLaboratoryTestId,
                        principalTable: "FillingLaboratoryTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateIssuers_ProjectId",
                table: "CertificateIssuers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CompactionDensityCertificateDetails_CompactionDensityCertificateId",
                table: "CompactionDensityCertificateDetails",
                column: "CompactionDensityCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CompactionDensityCertificateDetails_FillingLaboratoryTestId",
                table: "CompactionDensityCertificateDetails",
                column: "FillingLaboratoryTestId");

            migrationBuilder.CreateIndex(
                name: "IX_CompactionDensityCertificates_QuarryId",
                table: "CompactionDensityCertificates",
                column: "QuarryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompactionDensityCertificates_SewerLineId",
                table: "CompactionDensityCertificates",
                column: "SewerLineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteQualityCertificateDetails_ConcreteQualityCertificateId",
                table: "ConcreteQualityCertificateDetails",
                column: "ConcreteQualityCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteQualityCertificateDetails_SewerBoxId",
                table: "ConcreteQualityCertificateDetails",
                column: "SewerBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkFrontHeadId",
                table: "Employees",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentTypeId",
                table: "Equipment",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FillingLaboratoryTests_CertificateIssuerId",
                table: "FillingLaboratoryTests",
                column: "CertificateIssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterInterestGroups_InterestGroupId",
                table: "LetterInterestGroups",
                column: "InterestGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterIssuerTargets_IssuerTargetId",
                table: "LetterIssuerTargets",
                column: "IssuerTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_ReferenceId",
                table: "Letters",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_SewerGroupId",
                table: "ProductionDailyParts",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgedItems_ProjectBudgedItemParentId",
                table: "ProjectBudgedItems",
                column: "ProjectBudgedItemParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgedItems_ProjectBudgetCategoryId",
                table: "ProjectBudgedItems",
                column: "ProjectBudgetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgetCategories_ProjectId",
                table: "ProjectBudgetCategories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationZones_ProjectId",
                table: "QualificationZones",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Quarries_ProjectId",
                table: "Quarries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxes_SewerGroupId",
                table: "SewerBoxes",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkFrontId",
                table: "SewerGroups",
                column: "WorkFrontId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerLines_FinalSewerBoxId",
                table: "SewerLines",
                column: "FinalSewerBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerLines_InitialSewerBoxId",
                table: "SewerLines",
                column: "InitialSewerBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerLines_QualificationZoneId",
                table: "SewerLines",
                column: "QualificationZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerLines_SewerGroupId",
                table: "SewerLines",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPhases_ProjectId",
                table: "SystemPhases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFronts_SystemPhaseId",
                table: "WorkFronts",
                column: "SystemPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFronts_UserId",
                table: "WorkFronts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompactionDensityCertificateDetails");

            migrationBuilder.DropTable(
                name: "ConcreteQualityCertificateDetails");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "LetterInterestGroups");

            migrationBuilder.DropTable(
                name: "LetterIssuerTargets");

            migrationBuilder.DropTable(
                name: "ProductionDailyParts");

            migrationBuilder.DropTable(
                name: "ProjectBudgedItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CompactionDensityCertificates");

            migrationBuilder.DropTable(
                name: "FillingLaboratoryTests");

            migrationBuilder.DropTable(
                name: "ConcreteQualityCertificates");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "InterestGroup");

            migrationBuilder.DropTable(
                name: "IssuerTargets");

            migrationBuilder.DropTable(
                name: "Letters");

            migrationBuilder.DropTable(
                name: "ProjectBudgetCategories");

            migrationBuilder.DropTable(
                name: "Quarries");

            migrationBuilder.DropTable(
                name: "SewerLines");

            migrationBuilder.DropTable(
                name: "CertificateIssuers");

            migrationBuilder.DropTable(
                name: "SewerBoxes");

            migrationBuilder.DropTable(
                name: "QualificationZones");

            migrationBuilder.DropTable(
                name: "SewerGroups");

            migrationBuilder.DropTable(
                name: "Foremen");

            migrationBuilder.DropTable(
                name: "WorkFronts");

            migrationBuilder.DropTable(
                name: "SystemPhases");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
