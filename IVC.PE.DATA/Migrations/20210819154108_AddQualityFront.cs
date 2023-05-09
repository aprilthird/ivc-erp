using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddQualityFront : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QualityFrontId",
                table: "EquipmentCertificateRenewals",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QualityFronts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityFronts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_QualityFrontId",
                table: "EquipmentCertificateRenewals",
                column: "QualityFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificateRenewals_QualityFronts_QualityFrontId",
                table: "EquipmentCertificateRenewals",
                column: "QualityFrontId",
                principalTable: "QualityFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificateRenewals_QualityFronts_QualityFrontId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropTable(
                name: "QualityFronts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificateRenewals_QualityFrontId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "QualityFrontId",
                table: "EquipmentCertificateRenewals");
        }
    }
}
