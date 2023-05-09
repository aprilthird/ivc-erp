using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class PatternCalibrationRenewalId_EquipmentCertificateRenewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals",
                column: "PatternCalibrationRenewalId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificateRenewals_PatternCalibrationRenewals_PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals",
                column: "PatternCalibrationRenewalId",
                principalTable: "PatternCalibrationRenewals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificateRenewals_PatternCalibrationRenewals_PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificateRenewals_PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "PatternCalibrationRenewalId",
                table: "EquipmentCertificateRenewals");
        }
    }
}
