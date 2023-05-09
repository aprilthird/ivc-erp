using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEquipmentCertificateToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    CalibrationVerificationMethod = table.Column<int>(nullable: false),
                    CalibrationVerificationFrecuency = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificates_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCertificateRenewals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentCertificateId = table.Column<Guid>(nullable: false),
                    CertifyingEntity = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ValidityDate = table.Column<DateTime>(nullable: false),
                    ControlStatus = table.Column<int>(nullable: false),
                    OperationalStatus = table.Column<int>(nullable: false),
                    SituationStatus = table.Column<int>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertificateRenewals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificateRenewals_EquipmentCertificates_EquipmentCertificateId",
                        column: x => x.EquipmentCertificateId,
                        principalTable: "EquipmentCertificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificateRenewals_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_EquipmentCertificateId",
                table: "EquipmentCertificateRenewals",
                column: "EquipmentCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_SewerGroupId",
                table: "EquipmentCertificateRenewals",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_ProjectId",
                table: "EquipmentCertificates",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentCertificateRenewals");

            migrationBuilder.DropTable(
                name: "EquipmentCertificates");
        }
    }
}
