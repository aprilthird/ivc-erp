using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerSewerGroupRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "Workers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "Workers");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups",
                column: "WorkerId");
        }
    }
}
