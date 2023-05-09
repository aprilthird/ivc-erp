using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreatePatternCalibration_Renewal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatternCalibrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    NumberOfRenovations = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternCalibrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternCalibrations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatternCalibrationRenewals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PatternCalibrationId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EquipmentCertifyingEntityId = table.Column<Guid>(nullable: true),
                    Requestioner = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    RenewalOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternCalibrationRenewals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternCalibrationRenewals_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                        column: x => x.EquipmentCertifyingEntityId,
                        principalTable: "EquipmentCertifyingEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatternCalibrationRenewals_PatternCalibrations_PatternCalibrationId",
                        column: x => x.PatternCalibrationId,
                        principalTable: "PatternCalibrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatternCalibrationRenewals_EquipmentCertifyingEntityId",
                table: "PatternCalibrationRenewals",
                column: "EquipmentCertifyingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternCalibrationRenewals_PatternCalibrationId",
                table: "PatternCalibrationRenewals",
                column: "PatternCalibrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternCalibrations_ProjectId",
                table: "PatternCalibrations",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatternCalibrationRenewals");

            migrationBuilder.DropTable(
                name: "PatternCalibrations");
        }
    }
}
