using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldEntityAddSewerBoxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Pavement2In",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pavement3In",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pavement3InMixed",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PavementWidth",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxEndId",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxStartId",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "Pavement2In",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "Pavement3In",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "Pavement3InMixed",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PavementWidth",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxStartId",
                table: "SewerManifolds");
        }
    }
}
