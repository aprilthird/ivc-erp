using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SewerManifoldFor24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldFor24FirstParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NewSIGProcessId = table.Column<Guid>(nullable: false),
                    ReportUser = table.Column<string>(nullable: true),
                    OriginType = table.Column<int>(nullable: false),
                    NCOrigin = table.Column<int>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    BrandSupplier = table.Column<string>(nullable: true),
                    CodeReference = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    ResponsableUser = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor24FirstParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24FirstParts_NewSIGProcesses_NewSIGProcessId",
                        column: x => x.NewSIGProcessId,
                        principalTable: "NewSIGProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24FirstParts_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24FirstParts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_NewSIGProcessId",
                table: "SewerManifoldFor24FirstParts",
                column: "NewSIGProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                column: "SewerManifoldId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_SupplierId",
                table: "SewerManifoldFor24FirstParts",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldFor24FirstParts");
        }
    }
}
