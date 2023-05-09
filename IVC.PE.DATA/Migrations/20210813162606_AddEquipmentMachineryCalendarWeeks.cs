using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEquipmentMachineryCalendarWeeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryCalendarWeeks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WeekNumber = table.Column<int>(nullable: false),
                    WeekStart = table.Column<DateTime>(nullable: false),
                    WeekEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryCalendarWeeks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryCalendarWeeks");
        }
    }
}
