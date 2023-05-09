﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerGroupSchedulesAndActivitiesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupScheduleActivities_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupScheduleActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "HourRange",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SewerGroupSchedules",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FootageGoal",
                table: "SewerGroupSchedules",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "SewerGroupSchedules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "SewerGroupSchedules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerGroupSchedules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "FootageDaily",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_MeasurementUnitId",
                table: "SewerGroupSchedules",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_ProjectCalendarWeekId",
                table: "SewerGroupSchedules",
                column: "ProjectCalendarWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                column: "ProjectFormulaActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_WorkFrontHeadId",
                table: "SewerGroupSchedules",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupSchedules",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "SewerGroupSchedules",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                column: "ProjectFormulaActivityId",
                principalTable: "ProjectFormulaActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroupSchedules",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_MeasurementUnitId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_ProjectCalendarWeekId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_ProjectFormulaActivityId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_WorkFrontHeadId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "FootageGoal",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "FootageDaily",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "SewerGroupSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SewerGroupScheduleActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HourRange",
                table: "SewerGroupScheduleActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                column: "ProjectFormulaActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupScheduleActivities_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                column: "ProjectFormulaActivityId",
                principalTable: "ProjectFormulaActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
