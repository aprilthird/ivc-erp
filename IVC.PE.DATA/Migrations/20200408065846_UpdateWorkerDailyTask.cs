using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerDailyTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkerDailyTasks_SystemPhaseId",
                table: "WorkerDailyTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerDailyTasks_SystemPhases_SystemPhaseId",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "SystemPhaseId",
                table: "WorkerDailyTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "WorkerDailyTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemPhaseId",
                table: "WorkerDailyTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
