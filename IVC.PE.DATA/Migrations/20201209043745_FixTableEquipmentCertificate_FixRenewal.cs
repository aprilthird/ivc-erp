using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixTableEquipmentCertificate_FixRenewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificates_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "CertifyingEntity",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals",
                column: "EquipmentCertifyingEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificateRenewals_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals",
                column: "EquipmentCertifyingEntityId",
                principalTable: "EquipmentCertifyingEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificateRenewals_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificateRenewals_EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CertifyingEntity",
                table: "EquipmentCertificateRenewals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                column: "EquipmentCertifyingEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                column: "EquipmentCertifyingEntityId",
                principalTable: "EquipmentCertifyingEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
