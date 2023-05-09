using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FoldnigSoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftInsuranceFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftId = table.Column<Guid>(nullable: false),
                    StartDateInsurance = table.Column<DateTime>(nullable: true),
                    EndDateInsurance = table.Column<DateTime>(nullable: true),
                    InsuranceFileUrl = table.Column<string>(nullable: true),
                    OrderInsurance = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Days30 = table.Column<bool>(nullable: false),
                    Days15 = table.Column<bool>(nullable: false),
                    InsuranceEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftInsuranceFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySofts_EquipmentMachinerySoftId",
                        column: x => x.EquipmentMachinerySoftId,
                        principalTable: "EquipmentMachinerySofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftInsuranceFoldings_InsuranceEntity_InsuranceEntityId",
                        column: x => x.InsuranceEntityId,
                        principalTable: "InsuranceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftInsuranceFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftInsuranceFoldingApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftInsuranc~",
                        column: x => x.EquipmentMachinerySoftInsuranceFoldingId,
                        principalTable: "EquipmentMachinerySoftInsuranceFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldingId",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                column: "EquipmentMachinerySoftInsuranceFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftId",
                table: "EquipmentMachinerySoftInsuranceFoldings",
                column: "EquipmentMachinerySoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftInsuranceFoldings_InsuranceEntityId",
                table: "EquipmentMachinerySoftInsuranceFoldings",
                column: "InsuranceEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftInsuranceFoldingApplicationUser");

            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftInsuranceFoldings");
        }
    }
}
