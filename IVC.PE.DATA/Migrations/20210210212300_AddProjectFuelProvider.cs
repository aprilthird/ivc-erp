using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectFuelProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "FuelProviders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FuelProviders_ProjectId",
                table: "FuelProviders",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelProviders_Projects_ProjectId",
                table: "FuelProviders",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelProviders_Projects_ProjectId",
                table: "FuelProviders");

            migrationBuilder.DropIndex(
                name: "IX_FuelProviders_ProjectId",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "FuelProviders");
        }
    }
}
