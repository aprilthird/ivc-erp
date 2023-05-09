using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_NewSIGProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "NewSIGProcesses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_NewSIGProcesses_ProjectId",
                table: "NewSIGProcesses",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewSIGProcesses_Projects_ProjectId",
                table: "NewSIGProcesses",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewSIGProcesses_Projects_ProjectId",
                table: "NewSIGProcesses");

            migrationBuilder.DropIndex(
                name: "IX_NewSIGProcesses_ProjectId",
                table: "NewSIGProcesses");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "NewSIGProcesses");
        }
    }
}
