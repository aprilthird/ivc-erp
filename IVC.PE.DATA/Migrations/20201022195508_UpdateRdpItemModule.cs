using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRdpItemModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RdpItems_SewerGroups_SewerGroupId",
                table: "RdpItems");

            migrationBuilder.DropIndex(
                name: "IX_RdpItems_SewerGroupId",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "Accumulated",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "Contractual",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "RdpDate",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "StakeOut",
                table: "RdpItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Accumulated",
                table: "RdpItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Contractual",
                table: "RdpItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RdpDate",
                table: "RdpItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "RdpItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StakeOut",
                table: "RdpItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RdpItems_SewerGroupId",
                table: "RdpItems",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RdpItems_SewerGroups_SewerGroupId",
                table: "RdpItems",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
