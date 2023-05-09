using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateTableIsoStandard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "IsoStandards");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "IsoStandards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "IsoStandards");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "IsoStandards",
                type: "datetime2",
                nullable: true);
        }
    }
}
