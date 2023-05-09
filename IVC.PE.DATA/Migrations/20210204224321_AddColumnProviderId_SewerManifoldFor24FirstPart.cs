using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnProviderId_SewerManifoldFor24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Suppliers_SupplierId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24FirstParts_SupplierId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "BrandSupplier",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<string>(
                name: "BrandProvider",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "SewerManifoldFor24FirstParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerGroupSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    ProjectHabilitationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroupSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroupSchedules_ProjectHabilitations_ProjectHabilitationId",
                        column: x => x.ProjectHabilitationId,
                        principalTable: "ProjectHabilitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerGroupSchedules_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerGroupScheduleActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerGroupDailyScheduleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaActivityId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HourRange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroupScheduleActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroupScheduleActivities_ProjectFormulaActivities_ProjectFormulaActivityId",
                        column: x => x.ProjectFormulaActivityId,
                        principalTable: "ProjectFormulaActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerGroupScheduleActivities_SewerGroupSchedules_SewerGroupDailyScheduleId",
                        column: x => x.SewerGroupDailyScheduleId,
                        principalTable: "SewerGroupSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_ProviderId",
                table: "SewerManifoldFor24FirstParts",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_ProjectFormulaActivityId",
                table: "SewerGroupScheduleActivities",
                column: "ProjectFormulaActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupScheduleActivities_SewerGroupDailyScheduleId",
                table: "SewerGroupScheduleActivities",
                column: "SewerGroupDailyScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_ProjectHabilitationId",
                table: "SewerGroupSchedules",
                column: "ProjectHabilitationId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupSchedules_SewerGroupId",
                table: "SewerGroupSchedules",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Providers_ProviderId",
                table: "SewerManifoldFor24FirstParts",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Providers_ProviderId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropTable(
                name: "SewerGroupScheduleActivities");

            migrationBuilder.DropTable(
                name: "SewerGroupSchedules");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24FirstParts_ProviderId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "BrandProvider",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<string>(
                name: "BrandSupplier",
                table: "SewerManifoldFor24FirstParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "SewerManifoldFor24FirstParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_SupplierId",
                table: "SewerManifoldFor24FirstParts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Suppliers_SupplierId",
                table: "SewerManifoldFor24FirstParts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
