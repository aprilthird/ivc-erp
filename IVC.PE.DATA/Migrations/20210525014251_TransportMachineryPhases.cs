using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TransportMachineryPhases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineryPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineryPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineryPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineryPhases_ProjectPhaseId",
                table: "MachineryPhases",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportPhases_ProjectPhaseId",
                table: "TransportPhases",
                column: "ProjectPhaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineryPhases");

            migrationBuilder.DropTable(
                name: "TransportPhases");
        }
    }
}
