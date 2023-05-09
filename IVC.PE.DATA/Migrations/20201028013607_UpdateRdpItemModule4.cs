using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRdpItemModule4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RdpItemFootages_SewerGroups_SewerGroupId",
                table: "RdpItemFootages");

            migrationBuilder.DropForeignKey(
                name: "FK_RdpReports_WorkFrontHeads_WorkFrontHeadId",
                table: "RdpReports");

            migrationBuilder.DropIndex(
                name: "IX_RdpReports_WorkFrontHeadId",
                table: "RdpReports");

            migrationBuilder.DropIndex(
                name: "IX_RdpItemFootages_SewerGroupId",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "InstalledPipe",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "NumberOfOfficials",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "NumberOfOperators",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "NumberOfPawns",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "RdpDate",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "RdpItemFootages");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "RdpReports",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RdpReportId",
                table: "RdpItemFootages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RdpReports_ProjectPhaseId",
                table: "RdpReports",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_RdpReportId",
                table: "RdpItemFootages",
                column: "RdpReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_RdpItemFootages_RdpReports_RdpReportId",
                table: "RdpItemFootages",
                column: "RdpReportId",
                principalTable: "RdpReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RdpReports_ProjectPhases_ProjectPhaseId",
                table: "RdpReports",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RdpItemFootages_RdpReports_RdpReportId",
                table: "RdpItemFootages");

            migrationBuilder.DropForeignKey(
                name: "FK_RdpReports_ProjectPhases_ProjectPhaseId",
                table: "RdpReports");

            migrationBuilder.DropIndex(
                name: "IX_RdpReports_ProjectPhaseId",
                table: "RdpReports");

            migrationBuilder.DropIndex(
                name: "IX_RdpItemFootages_RdpReportId",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "RdpReports");

            migrationBuilder.DropColumn(
                name: "RdpReportId",
                table: "RdpItemFootages");

            migrationBuilder.AddColumn<float>(
                name: "InstalledPipe",
                table: "RdpReports",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfOfficials",
                table: "RdpReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfOperators",
                table: "RdpReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPawns",
                table: "RdpReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "RdpReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RdpDate",
                table: "RdpItemFootages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "RdpItemFootages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RdpReports_WorkFrontHeadId",
                table: "RdpReports",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_SewerGroupId",
                table: "RdpItemFootages",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RdpItemFootages_SewerGroups_SewerGroupId",
                table: "RdpItemFootages",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RdpReports_WorkFrontHeads_WorkFrontHeadId",
                table: "RdpReports",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
