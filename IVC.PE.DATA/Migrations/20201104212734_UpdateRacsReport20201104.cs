using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRacsReport20201104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "Ubication",
                table: "RacsReports");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RacsReports",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "RacsReports",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RacsReports_SewerGroupId",
                table: "RacsReports",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RacsReports_SewerGroups_SewerGroupId",
                table: "RacsReports",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacsReports_SewerGroups_SewerGroupId",
                table: "RacsReports");

            migrationBuilder.DropIndex(
                name: "IX_RacsReports_SewerGroupId",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "RacsReports");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "RacsReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ubication",
                table: "RacsReports",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
