using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnSewerManifoldToFor05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor05s_DischargeManifolds_DischargeManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor05s_DischargeManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "DischargeManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerManifoldFor05s",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_SewerManifoldId",
                table: "SewerManifoldFor05s",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor05s_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor05s",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor05s_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor05s_SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.AddColumn<Guid>(
                name: "DischargeManifoldId",
                table: "SewerManifoldFor05s",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_DischargeManifoldId",
                table: "SewerManifoldFor05s",
                column: "DischargeManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor05s_DischargeManifolds_DischargeManifoldId",
                table: "SewerManifoldFor05s",
                column: "DischargeManifoldId",
                principalTable: "DischargeManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
