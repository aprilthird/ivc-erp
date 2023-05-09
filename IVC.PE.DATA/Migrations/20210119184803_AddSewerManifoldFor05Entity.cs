using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerManifoldFor05Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldFor05s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    DischargeManifoldId = table.Column<Guid>(nullable: false),
                    CertificateNumber = table.Column<string>(nullable: true),
                    LayerNumber = table.Column<string>(nullable: true),
                    TestDate = table.Column<DateTime>(nullable: false),
                    WetDensity = table.Column<double>(nullable: false),
                    MoisturePercentage = table.Column<double>(nullable: false),
                    DryDensity = table.Column<double>(nullable: false),
                    PercentageRequiredCompaction = table.Column<double>(nullable: false),
                    PercentageRealCompactio = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    ShippingDate = table.Column<DateTime>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor05s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor05s_DischargeManifolds_DischargeManifoldId",
                        column: x => x.DischargeManifoldId,
                        principalTable: "DischargeManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor05s_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_DischargeManifoldId",
                table: "SewerManifoldFor05s",
                column: "DischargeManifoldId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_SewerManifoldId",
                table: "SewerManifoldFor05s",
                column: "SewerManifoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldFor05s");
        }
    }
}
