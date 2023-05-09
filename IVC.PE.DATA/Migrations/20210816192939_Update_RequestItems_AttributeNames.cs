using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestItems_AttributeNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasureInAtenttion",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasureInAtenttion",
                table: "PreRequestItems");

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAttention",
                table: "RequestItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestItemId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAttention",
                table: "PreRequestItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_PreRequestItemId",
                table: "RequestItems",
                column: "PreRequestItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_PreRequestItems_PreRequestItemId",
                table: "RequestItems",
                column: "PreRequestItemId",
                principalTable: "PreRequestItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_PreRequestItems_PreRequestItemId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_PreRequestItemId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasureInAttention",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "PreRequestItemId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasureInAttention",
                table: "PreRequestItems");

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAtenttion",
                table: "RequestItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAtenttion",
                table: "PreRequestItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId",
                principalTable: "PreRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
