using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerBoxEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerBoxes_SewerGroups_SewerGroupId",
                table: "SewerBoxes");

            migrationBuilder.DropIndex(
                name: "IX_SewerBoxes_SewerGroupId",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Bottom",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "DrainageArea",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "InputOutput",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "InternalDiameter",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "SewerBoxes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SewerBoxes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "ArrivalLevel",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BottomLevel",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoverLevel",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Diameter",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxes_Code",
                table: "SewerBoxes",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerBoxes_Code",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "ArrivalLevel",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "BottomLevel",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "CoverLevel",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "Diameter",
                table: "SewerBoxes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SewerBoxes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "SewerBoxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Bottom",
                table: "SewerBoxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Cover",
                table: "SewerBoxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DrainageArea",
                table: "SewerBoxes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "SewerBoxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InputOutput",
                table: "SewerBoxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InternalDiameter",
                table: "SewerBoxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "SewerBoxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "SewerBoxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxes_SewerGroupId",
                table: "SewerBoxes",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerBoxes_SewerGroups_SewerGroupId",
                table: "SewerBoxes",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
