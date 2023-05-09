using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewColumnsEquipmentCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateOwnerId",
                table: "EquipmentCertificates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateTypeId",
                table: "EquipmentCertificates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_EquipmentCertificateOwnerId",
                table: "EquipmentCertificates",
                column: "EquipmentCertificateOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_EquipmentCertificateTypeId",
                table: "EquipmentCertificates",
                column: "EquipmentCertificateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                column: "EquipmentCertifyingEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificates_EquipmentOwners_EquipmentCertificateOwnerId",
                table: "EquipmentCertificates",
                column: "EquipmentCertificateOwnerId",
                principalTable: "EquipmentOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertificateTypes_EquipmentCertificateTypeId",
                table: "EquipmentCertificates",
                column: "EquipmentCertificateTypeId",
                principalTable: "EquipmentCertificateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates",
                column: "EquipmentCertifyingEntityId",
                principalTable: "EquipmentCertifyingEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificates_EquipmentOwners_EquipmentCertificateOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertificateTypes_EquipmentCertificateTypeId",
                table: "EquipmentCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificates_EquipmentCertifyingEntities_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificates_EquipmentCertificateOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificates_EquipmentCertificateTypeId",
                table: "EquipmentCertificates");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificates_EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateTypeId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentCertifyingEntityId",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "EquipmentCertificates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
