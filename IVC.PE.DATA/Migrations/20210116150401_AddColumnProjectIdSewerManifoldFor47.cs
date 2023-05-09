using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnProjectIdSewerManifoldFor47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerManifoldFor47s",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor47s_ProjectId",
                table: "SewerManifoldFor47s",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor47s_Projects_ProjectId",
                table: "SewerManifoldFor47s",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor47s_Projects_ProjectId",
                table: "SewerManifoldFor47s");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor47s_ProjectId",
                table: "SewerManifoldFor47s");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerManifoldFor47s");
        }
    }
}
