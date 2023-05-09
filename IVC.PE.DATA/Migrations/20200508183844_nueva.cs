using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class nueva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinanceVariables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinanceVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BondRenovationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BondTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guarantor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceVariables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceVariables_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinanceVariables_BondRenovations_BondRenovationId",
                        column: x => x.BondRenovationId,
                        principalTable: "BondRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinanceVariables_BondTypes_BondTypeId",
                        column: x => x.BondTypeId,
                        principalTable: "BondTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinanceVariables_BankId",
                table: "FinanceVariables",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceVariables_BondRenovationId",
                table: "FinanceVariables",
                column: "BondRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceVariables_BondTypeId",
                table: "FinanceVariables",
                column: "BondTypeId");
        }
    }
}
