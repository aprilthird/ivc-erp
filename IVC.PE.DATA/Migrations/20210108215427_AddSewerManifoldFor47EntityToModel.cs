using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerManifoldFor47EntityToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldFor47s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    LengthOfDiggingN = table.Column<double>(nullable: false),
                    LengthOfDiggingSR = table.Column<double>(nullable: false),
                    LengthOfDiggingR = table.Column<double>(nullable: false),
                    WorkBookNumber = table.Column<int>(nullable: false),
                    WorkBookSeat = table.Column<int>(nullable: false),
                    WorkBookRegistryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor47s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor47s_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor47s_SewerManifoldId",
                table: "SewerManifoldFor47s",
                column: "SewerManifoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldFor47s");
        }
    }
}
