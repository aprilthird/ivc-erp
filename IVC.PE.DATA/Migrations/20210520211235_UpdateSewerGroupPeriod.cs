using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerGroupPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Destination",
                table: "SewerGroupPeriods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanEmployeeId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanWorkerId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCollaboratorId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkComponent",
                table: "SewerGroupPeriods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerGroupPeriods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkStructure",
                table: "SewerGroupPeriods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ProviderId",
                table: "SewerGroups",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_ForemanEmployeeId",
                table: "SewerGroupPeriods",
                column: "ForemanEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_ForemanWorkerId",
                table: "SewerGroupPeriods",
                column: "ForemanWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_ProjectCollaboratorId",
                table: "SewerGroupPeriods",
                column: "ProjectCollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_ProviderId",
                table: "SewerGroupPeriods",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_WorkFrontHeadId",
                table: "SewerGroupPeriods",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupPeriods_WorkFrontId",
                table: "SewerGroupPeriods",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_Employees_ForemanEmployeeId",
                table: "SewerGroupPeriods",
                column: "ForemanEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_Workers_ForemanWorkerId",
                table: "SewerGroupPeriods",
                column: "ForemanWorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_ProjectCollaborators_ProjectCollaboratorId",
                table: "SewerGroupPeriods",
                column: "ProjectCollaboratorId",
                principalTable: "ProjectCollaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_Providers_ProviderId",
                table: "SewerGroupPeriods",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroupPeriods",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupPeriods_WorkFronts_WorkFrontId",
                table: "SewerGroupPeriods",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Providers_ProviderId",
                table: "SewerGroups",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_Employees_ForemanEmployeeId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_Workers_ForemanWorkerId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_ProjectCollaborators_ProjectCollaboratorId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_Providers_ProviderId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupPeriods_WorkFronts_WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Providers_ProviderId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ProviderId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_ForemanEmployeeId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_ForemanWorkerId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_ProjectCollaboratorId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_ProviderId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_WorkFrontHeadId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupPeriods_WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "ForemanEmployeeId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "ForemanWorkerId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "ProjectCollaboratorId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "WorkComponent",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "SewerGroupPeriods");

            migrationBuilder.DropColumn(
                name: "WorkStructure",
                table: "SewerGroupPeriods");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SewerGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
