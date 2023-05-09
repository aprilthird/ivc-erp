using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_Stocks_AddProjectIdAndUnitPirceAndParcial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Parcial",
                table: "Stocks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Stocks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "Stocks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProjectId",
                table: "Stocks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ProjectId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Parcial",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Stocks");
        }
    }
}
