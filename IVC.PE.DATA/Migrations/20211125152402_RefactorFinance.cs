using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorFinance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Procedures",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IssueCost",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IssueCostUsd",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "IssueCost",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "IssueCostUsd",
                table: "BondRenovations");
        }
    }
}
