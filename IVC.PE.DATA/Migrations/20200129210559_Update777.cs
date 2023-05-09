using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update777 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Letters");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Letters",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DocumentTypes",
                table: "Letters",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Letters",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IssuerId",
                table: "Letters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Letters_EmployeeId",
                table: "Letters",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_IssuerId",
                table: "Letters",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Employees_EmployeeId",
                table: "Letters",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_IssuerTargets_IssuerId",
                table: "Letters",
                column: "IssuerId",
                principalTable: "IssuerTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Letters_Employees_EmployeeId",
            //    table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_IssuerTargets_IssuerId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_EmployeeId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_IssuerId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "DocumentTypes",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Letters");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Letters",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "Letters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
