using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SewerManifoldFor24SecondPart_For24SecondPartGallery_For24SecondPartAction_For24SecondPartEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SewerManifoldFor24SecondParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Decision = table.Column<int>(nullable: false),
                    Other = table.Column<string>(nullable: true),
                    LaborerQuantity = table.Column<int>(nullable: false),
                    LaborerHoursMan = table.Column<double>(nullable: false),
                    LaborerTotalHoursMan = table.Column<double>(nullable: false),
                    OfficialQuantity = table.Column<int>(nullable: false),
                    OfficialHoursMan = table.Column<double>(nullable: false),
                    OfficialTotalHoursMan = table.Column<double>(nullable: false),
                    OperatorQuantity = table.Column<int>(nullable: false),
                    OperatorHoursMan = table.Column<double>(nullable: false),
                    OperatorTotalHoursMan = table.Column<double>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor24SecondParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "For24SecondPartActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24SecondPartId = table.Column<Guid>(nullable: false),
                    ActionName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_For24SecondPartActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_For24SecondPartActions_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                        column: x => x.SewerManifoldFor24SecondPartId,
                        principalTable: "SewerManifoldFor24SecondParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "For24SecondPartEquipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24SecondPartId = table.Column<Guid>(nullable: false),
                    EquipmentName = table.Column<string>(nullable: true),
                    EquipmentQuantity = table.Column<int>(nullable: false),
                    EquipmentHours = table.Column<double>(nullable: false),
                    EquipmentTotalHours = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_For24SecondPartEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_For24SecondPartEquipments_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                        column: x => x.SewerManifoldFor24SecondPartId,
                        principalTable: "SewerManifoldFor24SecondParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "For24SecondPartGalleries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24SecondPartId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_For24SecondPartGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_For24SecondPartGalleries_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                        column: x => x.SewerManifoldFor24SecondPartId,
                        principalTable: "SewerManifoldFor24SecondParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_For24FirstPartGalleries_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.CreateIndex(
                name: "IX_For24SecondPartActions_SewerManifoldFor24SecondPartId",
                table: "For24SecondPartActions",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.CreateIndex(
                name: "IX_For24SecondPartEquipments_SewerManifoldFor24SecondPartId",
                table: "For24SecondPartEquipments",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.CreateIndex(
                name: "IX_For24SecondPartGalleries_SewerManifoldFor24SecondPartId",
                table: "For24SecondPartGalleries",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_For24FirstPartGalleries_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                column: "SewerManifoldFor24SecondPartId",
                principalTable: "SewerManifoldFor24SecondParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_For24FirstPartGalleries_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");

            migrationBuilder.DropTable(
                name: "For24SecondPartActions");

            migrationBuilder.DropTable(
                name: "For24SecondPartEquipments");

            migrationBuilder.DropTable(
                name: "For24SecondPartGalleries");

            migrationBuilder.DropTable(
                name: "SewerManifoldFor24SecondParts");

            migrationBuilder.DropIndex(
                name: "IX_For24FirstPartGalleries_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");

            migrationBuilder.DropColumn(
                name: "SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");
        }
    }
}
