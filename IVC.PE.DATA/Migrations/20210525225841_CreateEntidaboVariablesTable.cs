using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateEntidaboVariablesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntibadoVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplyId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntibadoVariables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntibadoVariables_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntibadoVariables_SupplyId",
                table: "EntibadoVariables",
                column: "SupplyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntibadoVariables");
        }
    }
}
