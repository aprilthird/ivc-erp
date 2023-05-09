using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddWorkerWorkPeriodsToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerWorkPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    PensionFundAdministratorId = table.Column<Guid>(nullable: true),
                    PensionFundUniqueIdentificationCode = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Origin = table.Column<int>(nullable: false),
                    Workgroup = table.Column<int>(nullable: false),
                    WorkerPositionId = table.Column<Guid>(nullable: true),
                    NumberOfChildren = table.Column<int>(nullable: false),
                    HasUnionFee = table.Column<bool>(nullable: false),
                    HasSctr = table.Column<bool>(nullable: false),
                    SctrHealthType = table.Column<int>(nullable: false),
                    SctrPensionType = table.Column<int>(nullable: false),
                    JudicialRetentionFixedAmmount = table.Column<decimal>(nullable: false),
                    JudicialRetentionPercentRate = table.Column<decimal>(nullable: false),
                    HasWeeklySettlement = table.Column<bool>(nullable: false),
                    LaborRegimen = table.Column<int>(nullable: false),
                    HasEPS = table.Column<bool>(nullable: false),
                    HasEsSaludPlusVida = table.Column<bool>(nullable: false),
                    CeaseDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerWorkPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerWorkPeriods_PensionFundAdministrators_PensionFundAdministratorId",
                        column: x => x.PensionFundAdministratorId,
                        principalTable: "PensionFundAdministrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerWorkPeriods_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerWorkPeriods_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerWorkPeriods_WorkPositions_WorkerPositionId",
                        column: x => x.WorkerPositionId,
                        principalTable: "WorkPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkPeriods_PensionFundAdministratorId",
                table: "WorkerWorkPeriods",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkPeriods_ProjectId",
                table: "WorkerWorkPeriods",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkPeriods_WorkerId",
                table: "WorkerWorkPeriods",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWorkPeriods_WorkerPositionId",
                table: "WorkerWorkPeriods",
                column: "WorkerPositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerWorkPeriods");
        }
    }
}
