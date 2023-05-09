using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldRemoveWorkFrontAndHeadProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerGroups_SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_WorkFronts_WorkFrontId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_WorkFrontHeadId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_WorkFrontId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "SewerManifolds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerGroupId",
                table: "SewerManifolds",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_WorkFrontHeadId",
                table: "SewerManifolds",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_WorkFrontId",
                table: "SewerManifolds",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerGroups_SewerGroupId",
                table: "SewerManifolds",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerManifolds",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_WorkFronts_WorkFrontId",
                table: "SewerManifolds",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
