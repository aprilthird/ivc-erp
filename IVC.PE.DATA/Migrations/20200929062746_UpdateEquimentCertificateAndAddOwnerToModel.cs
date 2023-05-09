using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateEquimentCertificateAndAddOwnerToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentOwnerId",
                table: "EquipmentCertificates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EquipmentOwners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
