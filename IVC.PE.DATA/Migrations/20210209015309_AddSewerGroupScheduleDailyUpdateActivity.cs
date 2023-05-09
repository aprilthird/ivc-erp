using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerGroupScheduleDailyUpdateActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_MeasurementUnitId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_ProjectFormulaActivityId",
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
                name: "ProjectFormulaActivityId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "FootageDaily",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SewerGroupScheduleActivities",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FootageGoal",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerGroupScheduleDailies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerGroupScheduleActivityId = table.Column<Guid>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    FootageDaily = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroupScheduleDailies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroupScheduleDailies_SewerGroupScheduleActivities_SewerGroupScheduleActivityId",
                        column: x => x.SewerGroupScheduleActivityId,
                        principalTable: "SewerGroupScheduleActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_MeasurementUnitId",
                table: "SewerGroupScheduleActivities",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                column: "ProjectFormulaActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleDailies_SewerGroupScheduleActivityId",
                table: "SewerGroupScheduleDailies",
                column: "SewerGroupScheduleActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupScheduleActivities_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupScheduleActivities",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupScheduleActivities_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                column: "ProjectFormulaActivityId",
                principalTable: "ProjectFormulaActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupScheduleActivities_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupScheduleActivities_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropTable(
                name: "SewerGroupScheduleDailies");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupScheduleActivities_MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupScheduleActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "FootageGoal",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SewerGroupSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FootageGoal",
                table: "SewerGroupSchedules",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "SewerGroupSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "FootageDaily",
                table: "SewerGroupScheduleActivities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "SewerGroupScheduleActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_MeasurementUnitId",
                table: "SewerGroupSchedules",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                column: "ProjectFormulaActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupSchedules",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupSchedules_ProjectFormulaActivities_ProjectFormulaActivityId",
                table: "SewerGroupSchedules",
                column: "ProjectFormulaActivityId",
                principalTable: "ProjectFormulaActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
