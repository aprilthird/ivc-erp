using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontSewerGroupModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFrontSewerGroups_SewerGroups_SewerGroupId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_WorkFrontSewerGroups_SewerGroupId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupPerioId",
                table: "WorkFrontSewerGroups",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupPeriodId",
                table: "WorkFrontSewerGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontSewerGroups_SewerGroupPeriodId",
                table: "WorkFrontSewerGroups",
                column: "SewerGroupPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFrontSewerGroups_SewerGroupPeriods_SewerGroupPeriodId",
                table: "WorkFrontSewerGroups",
                column: "SewerGroupPeriodId",
                principalTable: "SewerGroupPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFrontSewerGroups_SewerGroupPeriods_SewerGroupPeriodId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_WorkFrontSewerGroups_SewerGroupPeriodId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropColumn(
                name: "SewerGroupPerioId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropColumn(
                name: "SewerGroupPeriodId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "WorkFrontSewerGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontSewerGroups_SewerGroupId",
                table: "WorkFrontSewerGroups",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFrontSewerGroups_SewerGroups_SewerGroupId",
                table: "WorkFrontSewerGroups",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
