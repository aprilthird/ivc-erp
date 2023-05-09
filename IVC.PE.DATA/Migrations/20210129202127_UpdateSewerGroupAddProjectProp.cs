using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerGroupAddProjectProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ProjectId",
                table: "SewerGroups",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Projects_ProjectId",
                table: "SewerGroups",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Projects_ProjectId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ProjectId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerGroups");
        }
    }
}
