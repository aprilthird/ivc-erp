using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        /*    migrationBuilder.DropIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondLoads");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondLoads",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads",
                column: "EmployeeId");*/

        }
    }
}
