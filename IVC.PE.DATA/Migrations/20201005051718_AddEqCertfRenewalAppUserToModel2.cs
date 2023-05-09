using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEqCertfRenewalAppUserToModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentCertificateRenewalApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentCertificateRenewalId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertificateRenewalApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificateRenewalApplicationUsers_EquipmentCertificateRenewals_EquipmentCertificateRenewalId",
                        column: x => x.EquipmentCertificateRenewalId,
                        principalTable: "EquipmentCertificateRenewals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewalApplicationUsers_EquipmentCertificateRenewalId",
                table: "EquipmentCertificateRenewalApplicationUsers",
                column: "EquipmentCertificateRenewalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentCertificateRenewalApplicationUsers");
        }
    }
}
