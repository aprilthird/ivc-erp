using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BlueprintFoldingNullableDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlueprintDate",
                table: "Blueprints");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlueprintDate",
                table: "BlueprintFoldings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlueprintDate",
                table: "BlueprintFoldings");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlueprintDate",
                table: "Blueprints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
