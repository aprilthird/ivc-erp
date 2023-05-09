using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_NewSIGProcess_SewerManifoldFor24FirstPart_UserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewSIGProcesses_Employees_EmployeeId",
                table: "NewSIGProcesses");

            migrationBuilder.DropIndex(
                name: "IX_NewSIGProcesses_EmployeeId",
                table: "NewSIGProcesses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "NewSIGProcesses");

            migrationBuilder.AddColumn<string>(
                name: "ReportUserName",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableUserName",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportUserName",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ResponsableUserName",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "NewSIGProcesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_NewSIGProcesses_EmployeeId",
                table: "NewSIGProcesses",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewSIGProcesses_Employees_EmployeeId",
                table: "NewSIGProcesses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
