using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontSewerGroupsModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFrontSewerGroups",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_WorkFrontSewerGroups_WorkFrontId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkFrontSewerGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFrontSewerGroups",
                table: "WorkFrontSewerGroups",
                columns: new[] { "WorkFrontId", "SewerGroupPeriodId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFrontSewerGroups",
                table: "WorkFrontSewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "WorkFrontSewerGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFrontSewerGroups",
                table: "WorkFrontSewerGroups",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontSewerGroups_WorkFrontId",
                table: "WorkFrontSewerGroups",
                column: "WorkFrontId");
        }
    }
}
