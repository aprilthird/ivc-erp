using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateEquipmentCertificateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificates_EquipmentOwners_EquipmentOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.DropTable(
                name: "EquipmentOwners");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificates_EquipmentOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentOwnerId",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "EquipmentCertificates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentOwnerId",
                table: "EquipmentCertificates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EquipmentOwners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOwners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificates_EquipmentOwnerId",
                table: "EquipmentCertificates",
                column: "EquipmentOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificates_EquipmentOwners_EquipmentOwnerId",
                table: "EquipmentCertificates",
                column: "EquipmentOwnerId",
                principalTable: "EquipmentOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
