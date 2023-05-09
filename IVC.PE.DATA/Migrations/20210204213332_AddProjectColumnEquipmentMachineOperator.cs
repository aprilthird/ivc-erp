﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectColumnEquipmentMachineOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EquipmentMachineryOperators",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EquipmentMachineryOperators");
        }
    }
}
