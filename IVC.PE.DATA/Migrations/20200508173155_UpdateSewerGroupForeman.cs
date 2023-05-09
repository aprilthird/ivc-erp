using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerGroupForeman : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ForemanId",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanEmployeeId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanWorkerId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanEmployeeId",
                table: "SewerGroups",
                column: "ForemanEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Employees_ForemanEmployeeId",
                table: "SewerGroups",
                column: "ForemanEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Employees_ForemanEmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanEmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ForemanEmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanId",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanId",
                table: "SewerGroups",
                column: "ForemanId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
