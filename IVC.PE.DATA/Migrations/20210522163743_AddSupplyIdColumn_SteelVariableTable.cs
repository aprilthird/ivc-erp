using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSupplyIdColumn_SteelVariableTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "SteelVariables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SteelVariables_SupplyId",
                table: "SteelVariables",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SteelVariables_Supplies_SupplyId",
                table: "SteelVariables",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SteelVariables_Supplies_SupplyId",
                table: "SteelVariables");

            migrationBuilder.DropIndex(
                name: "IX_SteelVariables_SupplyId",
                table: "SteelVariables");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "SteelVariables");
        }
    }
}
