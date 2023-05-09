using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnDischargeManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificateRenewals_EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateId",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificates_EquipmentCertificateId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateId",
                principalTable: "EquipmentCertificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificates_EquipmentCertificateId",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateId",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateRenewalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificateRenewals_EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateRenewalId",
                principalTable: "EquipmentCertificateRenewals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
