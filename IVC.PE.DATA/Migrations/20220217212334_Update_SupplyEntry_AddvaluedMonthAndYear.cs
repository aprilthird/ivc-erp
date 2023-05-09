using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntry_AddvaluedMonthAndYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isValued",
                table: "SupplyEntries",
                newName: "IsValued");

            migrationBuilder.AddColumn<int>(
                name: "ValuedMonth",
                table: "SupplyEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValuedYear",
                table: "SupplyEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProjectCalendarWeekFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCalendarWeekFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCalendarWeekFiles_ProjectCalendarWeeks_ProjectCalendarWeekId",
                        column: x => x.ProjectCalendarWeekId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCalendarWeekFiles_ProjectCalendarWeekId",
                table: "ProjectCalendarWeekFiles",
                column: "ProjectCalendarWeekId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCalendarWeekFiles");

            migrationBuilder.DropColumn(
                name: "ValuedMonth",
                table: "SupplyEntries");

            migrationBuilder.DropColumn(
                name: "ValuedYear",
                table: "SupplyEntries");

            migrationBuilder.RenameColumn(
                name: "IsValued",
                table: "SupplyEntries",
                newName: "isValued");
        }
    }
}
