using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddMachineryTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionalType",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<int>(
                name: "MachineryType",
                table: "EquipmentMachineryOperators",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineryType",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<Guid>(
                name: "ConditionalType",
                table: "EquipmentMachineryOperators",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
