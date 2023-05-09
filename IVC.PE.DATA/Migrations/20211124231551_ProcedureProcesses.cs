using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProcedureProcesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Processes_ProcessId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_ProcessId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "Procedures");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Procedures",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProceduresProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProceduresProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProceduresProcesses_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProceduresProcesses_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresProcesses_ProcedureId",
                table: "ProceduresProcesses",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduresProcesses_ProcessId",
                table: "ProceduresProcesses",
                column: "ProcessId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProceduresProcesses");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Procedures");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessId",
                table: "Procedures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_ProcessId",
                table: "Procedures",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Processes_ProcessId",
                table: "Procedures",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
