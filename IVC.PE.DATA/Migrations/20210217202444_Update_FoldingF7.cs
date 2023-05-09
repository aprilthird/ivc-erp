using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_FoldingF7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoldingF7s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductionDailyPartId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ExcavatedLength = table.Column<double>(nullable: false),
                    InstalledLength = table.Column<double>(nullable: false),
                    PaddedLength = table.Column<double>(nullable: false),
                    GranularBaseLength = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldingF7s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldingF7s_ProductionDailyParts_ProductionDailyPartId",
                        column: x => x.ProductionDailyPartId,
                        principalTable: "ProductionDailyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldingF7s_ProductionDailyPartId",
                table: "FoldingF7s",
                column: "ProductionDailyPartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldingF7s");
        }
    }
}
