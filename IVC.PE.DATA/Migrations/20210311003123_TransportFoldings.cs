using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TransportFoldings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentProviderFoldingId = table.Column<Guid>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    EquipmentYear = table.Column<string>(nullable: true),
                    EquipmentPlate = table.Column<string>(nullable: true),
                    EquipmentSerie = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ServiceCondition = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransports_EquipmentProviderFoldings_EquipmentProviderFoldingId",
                        column: x => x.EquipmentProviderFoldingId,
                        principalTable: "EquipmentProviderFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransports_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportInsuranceFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportId = table.Column<Guid>(nullable: false),
                    StartDateInsurance = table.Column<DateTime>(nullable: true),
                    EndDateInsurance = table.Column<DateTime>(nullable: true),
                    InsuranceFileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportInsuranceFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportInsuranceFoldings_EquipmentMachineryTransports_EquipmentMachineryTransportId",
                        column: x => x.EquipmentMachineryTransportId,
                        principalTable: "EquipmentMachineryTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportSOATFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportId = table.Column<Guid>(nullable: false),
                    StartDateSOAT = table.Column<DateTime>(nullable: true),
                    EndDateSOAT = table.Column<DateTime>(nullable: true),
                    SOATFileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportSOATFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportSOATFoldings_EquipmentMachineryTransports_EquipmentMachineryTransportId",
                        column: x => x.EquipmentMachineryTransportId,
                        principalTable: "EquipmentMachineryTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportTechnicalRevisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportId = table.Column<Guid>(nullable: false),
                    StartDateTechnicalRevision = table.Column<DateTime>(nullable: true),
                    EndDateTechnicalRevision = table.Column<DateTime>(nullable: true),
                    TechnicalRevisionFileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportTechnicalRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportTechnicalRevisions_EquipmentMachineryTransports_EquipmentMachineryTransportId",
                        column: x => x.EquipmentMachineryTransportId,
                        principalTable: "EquipmentMachineryTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportInsuranceFoldingApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportInsuranceFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportInsuranceFoldingApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportInsuranceFoldingApplicationUsers_EquipmentMachineryTransportInsuranceFoldings_EquipmentMachineryT~",
                        column: x => x.EquipmentMachineryTransportInsuranceFoldingId,
                        principalTable: "EquipmentMachineryTransportInsuranceFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportSOATFoldingApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportSOATFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportSOATFoldingApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportSOATFoldingApplicationUsers_EquipmentMachineryTransportSOATFoldings_EquipmentMachineryTransportSO~",
                        column: x => x.EquipmentMachineryTransportSOATFoldingId,
                        principalTable: "EquipmentMachineryTransportSOATFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportTechnicalRevisionFoldingApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportTechnicalRevisionFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportTechnicalRevisionFoldingApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportTechnicalRevisionFoldingApplications_EquipmentMachineryTransportTechnicalRevisions_EquipmentMachi~",
                        column: x => x.EquipmentMachineryTransportTechnicalRevisionFoldingId,
                        principalTable: "EquipmentMachineryTransportTechnicalRevisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportInsuranceFoldingApplicationUsers_EquipmentMachineryTransportInsuranceFoldingId",
                table: "EquipmentMachineryTransportInsuranceFoldingApplicationUsers",
                column: "EquipmentMachineryTransportInsuranceFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportInsuranceFoldings_EquipmentMachineryTransportId",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                column: "EquipmentMachineryTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransports_EquipmentProviderFoldingId",
                table: "EquipmentMachineryTransports",
                column: "EquipmentProviderFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransports_EquipmentProviderId",
                table: "EquipmentMachineryTransports",
                column: "EquipmentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransports_ProjectId",
                table: "EquipmentMachineryTransports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportSOATFoldingApplicationUsers_EquipmentMachineryTransportSOATFoldingId",
                table: "EquipmentMachineryTransportSOATFoldingApplicationUsers",
                column: "EquipmentMachineryTransportSOATFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportSOATFoldings_EquipmentMachineryTransportId",
                table: "EquipmentMachineryTransportSOATFoldings",
                column: "EquipmentMachineryTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportTechnicalRevisionFoldingApplications_EquipmentMachineryTransportTechnicalRevisionFoldingId",
                table: "EquipmentMachineryTransportTechnicalRevisionFoldingApplications",
                column: "EquipmentMachineryTransportTechnicalRevisionFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportTechnicalRevisions_EquipmentMachineryTransportId",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                column: "EquipmentMachineryTransportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportInsuranceFoldingApplicationUsers");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportSOATFoldingApplicationUsers");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportTechnicalRevisionFoldingApplications");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportSOATFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransports");
        }
    }
}
