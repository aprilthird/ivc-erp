using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TestDoubleFKSameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificate2Id",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_EquipmentCertificate2Id",
                table: "DischargeManifolds",
                column: "EquipmentCertificate2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificates_EquipmentCertificate2Id",
                table: "DischargeManifolds",
                column: "EquipmentCertificate2Id",
                principalTable: "EquipmentCertificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificates_EquipmentCertificate2Id",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_EquipmentCertificate2Id",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificate2Id",
                table: "DischargeManifolds");
        }
    }
}
