using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontSewerGroupModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_WorkFronts_WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "SewerGroupPerioId",
                table: "WorkFrontSewerGroups");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.AlterColumn<Guid>(
                name: "SewerGroupPeriodId",
                table: "WorkFrontSewerGroups",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SewerGroupPeriodId",
                table: "WorkFrontSewerGroups",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupPerioId",
                table: "WorkFrontSewerGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerGroupPeriods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_WorkFrontId",
                table: "SewerGroupPeriods",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_WorkFronts_WorkFrontId",
                table: "SewerGroupPeriods",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
