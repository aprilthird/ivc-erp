using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateEmployeeEntityMissingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PensionFundAdministrators_PensionFundAdministratorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PensionFundAdministratorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PensionFundAdministratorId",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PensionFundAdministratorId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PensionFundAdministratorId",
                table: "Employees",
                column: "CurrentPositionId");

            migrationBuilder.AddForeignKey(
                name: "IX_Employees_PensionFundAdministratorId",
                table: "Employees",
                column: "PensionFundAdministratorId",
                principalTable: "PensionFundAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
