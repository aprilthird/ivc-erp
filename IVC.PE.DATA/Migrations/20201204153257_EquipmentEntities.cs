using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentCertificateTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    PillColor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertificateTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificateTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCertifyingEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CertifyingEntityName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertifyingEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertifyingEntities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentOwners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentOwners_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentResponsibles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentResponsibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentResponsibles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateTypes_ProjectId",
                table: "EquipmentCertificateTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertifyingEntities_ProjectId",
                table: "EquipmentCertifyingEntities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOwners_ProjectId",
                table: "EquipmentOwners",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentResponsibles_ProjectId",
                table: "EquipmentResponsibles",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentCertificateTypes");

            migrationBuilder.DropTable(
                name: "EquipmentCertifyingEntities");

            migrationBuilder.DropTable(
                name: "EquipmentOwners");

            migrationBuilder.DropTable(
                name: "EquipmentResponsibles");
        }
    }
}
