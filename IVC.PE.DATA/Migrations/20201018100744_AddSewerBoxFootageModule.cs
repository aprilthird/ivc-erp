using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerBoxFootageModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerBoxFootages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Range = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerBoxFootages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SewerBoxFootageItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerBoxFootageId = table.Column<Guid>(nullable: false),
                    Group = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RealFootage = table.Column<decimal>(nullable: false),
                    TechnicalRecordFootage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerBoxFootageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerBoxFootageItems_SewerBoxFootages_SewerBoxFootageId",
                        column: x => x.SewerBoxFootageId,
                        principalTable: "SewerBoxFootages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxFootageItems_SewerBoxFootageId",
                table: "SewerBoxFootageItems",
                column: "SewerBoxFootageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerBoxFootageItems");

            migrationBuilder.DropTable(
                name: "SewerBoxFootages");
        }
    }
}
