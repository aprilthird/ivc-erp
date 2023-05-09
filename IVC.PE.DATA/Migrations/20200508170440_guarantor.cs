using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class guarantor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         /*   migrationBuilder.DropColumn(
                name: "Guarantor",
                table: "BondLoads");

            migrationBuilder.AddColumn<Guid>(
                name: "BondGuarantorId",
                table: "BondLoads",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondLoads",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BondGuarantors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondGuarantors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BondGuarantorId",
                table: "BondLoads",
                column: "BondGuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondLoads_BondGuarantors_BondGuarantorId",
                table: "BondLoads",
                column: "BondGuarantorId",
                principalTable: "BondGuarantors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
            /*migrationBuilder.DropForeignKey(
                name: "FK_BondLoads_BondGuarantors_BondGuarantorId",
                table: "BondLoads");

            migrationBuilder.DropForeignKey(
                name: "FK_BondLoads_Employees_EmployeeId",
                table: "BondLoads");

            migrationBuilder.DropTable(
                name: "BondGuarantors");

            migrationBuilder.DropIndex(
                name: "IX_BondLoads_BondGuarantorId",
                table: "BondLoads");

            migrationBuilder.DropIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads");

            migrationBuilder.DropColumn(
                name: "BondGuarantorId",
                table: "BondLoads");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondLoads");

            migrationBuilder.AddColumn<string>(
                name: "Guarantor",
                table: "BondLoads",
                type: "nvarchar(max)",
                nullable: true);*/
        }
    }
}
