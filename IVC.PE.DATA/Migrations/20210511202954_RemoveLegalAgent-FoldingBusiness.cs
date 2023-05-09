using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveLegalAgentFoldingBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate5",
                table: "Businesses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate2",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate3",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate4",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate5",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive2",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive3",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive4",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive5",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent2",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent3",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent4",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent5",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate2",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate3",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate4",
                table: "Businesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate5",
                table: "Businesses",
                type: "datetime2",
                nullable: true);
        }
    }
}
