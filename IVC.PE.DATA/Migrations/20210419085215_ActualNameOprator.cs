﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ActualNameOprator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualName",
                table: "EquipmentMachineryOperators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualName",
                table: "EquipmentMachineryOperators");
        }
    }
}
