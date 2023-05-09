using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondLoads",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondLoads_Employees_EmployeeId",
                table: "BondLoads",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_BondLoads_Employees_EmployeeId",
                table: "BondLoads");

            migrationBuilder.DropIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondLoads");*/
        }
    }
}
