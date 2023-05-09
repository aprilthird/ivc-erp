using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorEquipmentMach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachSOATFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachId = table.Column<Guid>(nullable: false),
                    StartDateSOAT = table.Column<DateTime>(nullable: true),
                    EndDateSOAT = table.Column<DateTime>(nullable: true),
                    SOATFileUrl = table.Column<string>(nullable: true),
                    SoatOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachSOATFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachSOATFoldings_EquipmentMachs_EquipmentMachId",
                        column: x => x.EquipmentMachId,
                        principalTable: "EquipmentMachs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachTechnicalRevisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachId = table.Column<Guid>(nullable: false),
                    StartDateTechnicalRevision = table.Column<DateTime>(nullable: true),
                    EndDateTechnicalRevision = table.Column<DateTime>(nullable: true),
                    TechnicalRevisionFileUrl = table.Column<string>(nullable: true),
                    TechnicalOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachTechnicalRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachTechnicalRevisions_EquipmentMachs_EquipmentMachId",
                        column: x => x.EquipmentMachId,
                        principalTable: "EquipmentMachs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachSOATFoldingApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachSOATFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachSOATFoldingApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachSOATFoldingApplicationUsers_EquipmentMachSOATFoldings_EquipmentMachSOATFoldingId",
                        column: x => x.EquipmentMachSOATFoldingId,
                        principalTable: "EquipmentMachSOATFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachTechnicalRevisionFoldingApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachTechnicalRevisionFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachTechnicalRevisionFoldingApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachTechnicalRevisionFoldingApplications_EquipmentMachTechnicalRevisions_EquipmentMachTechnicalRevisionFoldingId",
                        column: x => x.EquipmentMachTechnicalRevisionFoldingId,
                        principalTable: "EquipmentMachTechnicalRevisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachSOATFoldingApplicationUsers_EquipmentMachSOATFoldingId",
                table: "EquipmentMachSOATFoldingApplicationUsers",
                column: "EquipmentMachSOATFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachSOATFoldings_EquipmentMachId",
                table: "EquipmentMachSOATFoldings",
                column: "EquipmentMachId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachTechnicalRevisionFoldingApplications_EquipmentMachTechnicalRevisionFoldingId",
                table: "EquipmentMachTechnicalRevisionFoldingApplications",
                column: "EquipmentMachTechnicalRevisionFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachTechnicalRevisions_EquipmentMachId",
                table: "EquipmentMachTechnicalRevisions",
                column: "EquipmentMachId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachSOATFoldingApplicationUsers");

            migrationBuilder.DropTable(
                name: "EquipmentMachTechnicalRevisionFoldingApplications");

            migrationBuilder.DropTable(
                name: "EquipmentMachSOATFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachTechnicalRevisions");
        }
    }
}
