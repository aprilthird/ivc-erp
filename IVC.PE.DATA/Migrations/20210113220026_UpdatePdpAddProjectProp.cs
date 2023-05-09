using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePdpAddProjectProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_ProjectId",
                table: "ProductionDailyParts",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDailyParts_Projects_ProjectId",
                table: "ProductionDailyParts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDailyParts_Projects_ProjectId",
                table: "ProductionDailyParts");

            migrationBuilder.DropIndex(
                name: "IX_ProductionDailyParts_ProjectId",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProductionDailyParts");
        }
    }
}
