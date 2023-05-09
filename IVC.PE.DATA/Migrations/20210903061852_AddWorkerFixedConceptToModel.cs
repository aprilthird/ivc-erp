using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddWorkerFixedConceptToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerFixedConcepts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    PayrollConceptId = table.Column<Guid>(nullable: false),
                    FixedValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerFixedConcepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerFixedConcepts_PayrollConcepts_PayrollConceptId",
                        column: x => x.PayrollConceptId,
                        principalTable: "PayrollConcepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerFixedConcepts_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerFixedConcepts_PayrollConceptId",
                table: "WorkerFixedConcepts",
                column: "PayrollConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerFixedConcepts_WorkerId",
                table: "WorkerFixedConcepts",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerFixedConcepts");
        }
    }
}
