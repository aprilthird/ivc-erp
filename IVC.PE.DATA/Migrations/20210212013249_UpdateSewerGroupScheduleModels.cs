using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerGroupScheduleModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupScheduleActivities_MeasurementUnits_MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupSchedules_ProjectHabilitations_ProjectHabilitationId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropTable(
                name: "SewerGroupScheduleDailies");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupSchedules_ProjectHabilitationId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupScheduleActivities_MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "ProjectHabilitationId",
                table: "SewerGroupSchedules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "FootageGoal",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerGroupScheduleActivities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerGroupScheduleActivityDailies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerGroupScheduleActivityId = table.Column<Guid>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    FootageDaily = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroupScheduleActivityDailies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroupScheduleActivityDailies_SewerGroupScheduleActivities_SewerGroupScheduleActivityId",
                        column: x => x.SewerGroupScheduleActivityId,
                        principalTable: "SewerGroupScheduleActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_SewerManifoldId",
                table: "SewerGroupScheduleActivities",
                column: "SewerManifoldId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivityDailies_SewerGroupScheduleActivityId",
                table: "SewerGroupScheduleActivityDailies",
                column: "SewerGroupScheduleActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroupScheduleActivities_SewerManifolds_SewerManifoldId",
                table: "SewerGroupScheduleActivities",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroupScheduleActivities_SewerManifolds_SewerManifoldId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropTable(
                name: "SewerGroupScheduleActivityDailies");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroupScheduleActivities_SewerManifoldId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "SewerGroupScheduleActivities");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectHabilitationId",
                table: "SewerGroupSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SewerGroupScheduleActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FootageGoal",
                table: "SewerGroupScheduleActivities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "SewerGroupScheduleActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerGroupScheduleDailies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FootageDaily = table.Column<double>(type: "float", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SewerGroupScheduleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_SewerGroupSchedules_ProjectHabilitationId",
                table: "SewerGroupSchedules",
                column: "ProjectHabilitationId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_MeasurementUnitId",
                table: "SewerGroupScheduleActivities",
                column: "MeasurementUnitId");

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
                name: "FK_SewerGroupSchedules_ProjectHabilitations_ProjectHabilitationId",
                table: "SewerGroupSchedules",
                column: "ProjectHabilitationId",
                principalTable: "ProjectHabilitations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
