using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestItems_AddOptionalPreRequestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId",
                principalTable: "PreRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "PreRequestId",
                table: "RequestItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
