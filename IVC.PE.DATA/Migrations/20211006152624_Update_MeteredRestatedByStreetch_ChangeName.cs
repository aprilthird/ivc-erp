using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_MeteredRestatedByStreetch_ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_BudgetTitles_BudgetTittleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropIndex(
                name: "IX_MeteredsRestatedByStreetchs_BudgetTittleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropColumn(
                name: "BudgetTittleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "MeteredsRestatedByStreetchs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_BudgetTitleId",
                table: "MeteredsRestatedByStreetchs",
                column: "BudgetTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_BudgetTitles_BudgetTitleId",
                table: "MeteredsRestatedByStreetchs",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_BudgetTitles_BudgetTitleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropIndex(
                name: "IX_MeteredsRestatedByStreetchs_BudgetTitleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTittleId",
                table: "MeteredsRestatedByStreetchs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_BudgetTittleId",
                table: "MeteredsRestatedByStreetchs",
                column: "BudgetTittleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_BudgetTitles_BudgetTittleId",
                table: "MeteredsRestatedByStreetchs",
                column: "BudgetTittleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
