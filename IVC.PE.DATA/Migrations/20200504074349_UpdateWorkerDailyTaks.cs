using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerDailyTaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkerDailyTasks_SewerGroupId",
                table: "WorkerDailyTasks",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerDailyTasks_SewerGroups_SewerGroupId",
                table: "WorkerDailyTasks",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerDailyTasks_SewerGroups_SewerGroupId",
                table: "WorkerDailyTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkerDailyTasks_SewerGroupId",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "WorkerDailyTasks");
        }
    }
}
