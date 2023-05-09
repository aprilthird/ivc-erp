using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class PatternCalibrationResponsibles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatternCalibrationRenewalApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PatternCalibrationRenewalId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternCalibrationRenewalApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternCalibrationRenewalApplicationUsers_PatternCalibrationRenewals_PatternCalibrationRenewalId",
                        column: x => x.PatternCalibrationRenewalId,
                        principalTable: "PatternCalibrationRenewals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatternCalibrationRenewalApplicationUsers_PatternCalibrationRenewalId",
                table: "PatternCalibrationRenewalApplicationUsers",
                column: "PatternCalibrationRenewalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatternCalibrationRenewalApplicationUsers");
        }
    }
}
