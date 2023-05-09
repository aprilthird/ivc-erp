using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondsModel13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondRenovations_Employees_EmployeeId",
                table: "BondRenovations");

            migrationBuilder.DropIndex(
                name: "IX_BondRenovations_EmployeeId",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondRenovations");

            migrationBuilder.CreateTable(
                name: "BondRenovationApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BondRenovationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondRenovationApplicationUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BondRenovationApplicationUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondRenovations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondRenovations_EmployeeId",
                table: "BondRenovations",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondRenovations_Employees_EmployeeId",
                table: "BondRenovations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
