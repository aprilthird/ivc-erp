using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_diverseInput_DeleteUnitString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "DiverseInputs");

            migrationBuilder.AlterColumn<Guid>(
                name: "MeasurementUnitId",
                table: "DiverseInputs",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MeasurementUnitId",
                table: "DiverseInputs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "DiverseInputs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
