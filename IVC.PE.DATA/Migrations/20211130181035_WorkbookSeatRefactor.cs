using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkbookSeatRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkbookTypeId",
                table: "WorkbookSeats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkbookSeats_WorkbookTypeId",
                table: "WorkbookSeats",
                column: "WorkbookTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkbookSeats_WorkbookTypes_WorkbookTypeId",
                table: "WorkbookSeats",
                column: "WorkbookTypeId",
                principalTable: "WorkbookTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkbookSeats_WorkbookTypes_WorkbookTypeId",
                table: "WorkbookSeats");

            migrationBuilder.DropIndex(
                name: "IX_WorkbookSeats_WorkbookTypeId",
                table: "WorkbookSeats");

            migrationBuilder.DropColumn(
                name: "WorkbookTypeId",
                table: "WorkbookSeats");
        }
    }
}
