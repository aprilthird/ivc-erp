using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnProjectIdDischargeManifoldFor01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "DischargeManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_ProjectId",
                table: "DischargeManifolds",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_Projects_ProjectId",
                table: "DischargeManifolds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_Projects_ProjectId",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_ProjectId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DischargeManifolds");
        }
    }
}
