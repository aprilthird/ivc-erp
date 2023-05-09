using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DischargeSewerManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LenghtBetweenAxisH",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "LevelArrivalJ",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "LevelBottomI",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "LevelBottomJ",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "LevelTopI",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "LevelTopJ",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "DischargeManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_SewerManifoldId",
                table: "DischargeManifolds",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_SewerManifolds_SewerManifoldId",
                table: "DischargeManifolds",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_SewerManifolds_SewerManifoldId",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_SewerManifoldId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<decimal>(
                name: "LenghtBetweenAxisH",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LevelArrivalJ",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LevelBottomI",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LevelBottomJ",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LevelTopI",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LevelTopJ",
                table: "DischargeManifolds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "DischargeManifolds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
