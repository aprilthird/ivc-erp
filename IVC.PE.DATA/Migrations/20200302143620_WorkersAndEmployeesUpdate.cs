using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkersAndEmployeesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Employees_AspNetUsers_UserId",
            //    table: "Employees");

            //migrationBuilder.DropIndex(
            //    name: "IX_Employees_UserId",
            //    table: "Employees");

            migrationBuilder.DropColumn(
                name: "CurrentPosition",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EntryPosition",
                table: "Employees");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPositionId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntryPositionId",
                table: "Employees",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EmployeePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPositions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CurrentPositionId",
                table: "Employees",
                column: "CurrentPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EntryPositionId",
                table: "Employees",
                column: "EntryPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeePositions_CurrentPositionId",
                table: "Employees",
                column: "CurrentPositionId",
                principalTable: "EmployeePositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeePositions_EntryPositionId",
                table: "Employees",
                column: "EntryPositionId",
                principalTable: "EmployeePositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePositions_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePositions_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeePositions");

            migrationBuilder.DropTable(
                name: "WorkerPositions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EntryPositionId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "CurrentPosition",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryPosition",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
