using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddNewTrainingSessionAndControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SessionDate = table.Column<DateTime>(nullable: false),
                    TrainingTopicId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_TrainingTopics_TrainingTopicId",
                        column: x => x.TrainingTopicId,
                        principalTable: "TrainingTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessionWorkerEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingSessionId = table.Column<Guid>(nullable: false),
                    TrainingResultStatusId = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: true),
                    Observation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessionWorkerEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSessionWorkerEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingSessionWorkerEmployees_TrainingResultStatus_TrainingResultStatusId",
                        column: x => x.TrainingResultStatusId,
                        principalTable: "TrainingResultStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingSessionWorkerEmployees_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingSessionWorkerEmployees_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_TrainingTopicId",
                table: "TrainingSessions",
                column: "TrainingTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_WorkFrontId",
                table: "TrainingSessions",
                column: "WorkFrontId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionWorkerEmployees_EmployeeId",
                table: "TrainingSessionWorkerEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionWorkerEmployees_TrainingResultStatusId",
                table: "TrainingSessionWorkerEmployees",
                column: "TrainingResultStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionWorkerEmployees_TrainingSessionId",
                table: "TrainingSessionWorkerEmployees",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionWorkerEmployees_WorkerId",
                table: "TrainingSessionWorkerEmployees",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingSessionWorkerEmployees");

            migrationBuilder.DropTable(
                name: "TrainingSessions");
        }
    }
}
