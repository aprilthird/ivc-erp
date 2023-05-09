using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UserUpdateWorkPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePositions_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePositions_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkerPositions_WorkerPositionId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "EmployeePositions");

            migrationBuilder.DropTable(
                name: "WorkerPositions");

            migrationBuilder.CreateTable(
                name: "WorkPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPositions", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkPositions_CurrentPositionId",
                table: "Employees",
                column: "CurrentPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkPositions_EntryPositionId",
                table: "Employees",
                column: "EntryPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_WorkPositions_WorkerPositionId",
                table: "Workers",
                column: "WorkerPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkPositions_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkPositions_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkPositions_WorkerPositionId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "WorkPositions");

            migrationBuilder.CreateTable(
                name: "EmployeePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPositions", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_WorkerPositions_WorkerPositionId",
                table: "Workers",
                column: "WorkerPositionId",
                principalTable: "WorkerPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
