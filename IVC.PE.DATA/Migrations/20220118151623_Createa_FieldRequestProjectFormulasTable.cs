using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Createa_FieldRequestProjectFormulasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WarehouseCurrentMetered",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FieldRequestProjectFormulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FieldRequestId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldRequestProjectFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldRequestProjectFormulas_FieldRequests_FieldRequestId",
                        column: x => x.FieldRequestId,
                        principalTable: "FieldRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequestProjectFormulas_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestProjectFormulas_FieldRequestId",
                table: "FieldRequestProjectFormulas",
                column: "FieldRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestProjectFormulas_ProjectFormulaId",
                table: "FieldRequestProjectFormulas",
                column: "ProjectFormulaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldRequestProjectFormulas");

            migrationBuilder.DropColumn(
                name: "WarehouseCurrentMetered",
                table: "GoalBudgetInputs");
        }
    }
}
