using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddFolding05EntityAndEditSewerManifoldFor05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor05s_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor05s_SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "DryDensity",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "MoisturePercentage",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "PercentageRealCompactio",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "PercentageRequiredCompaction",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "WetDensity",
                table: "SewerManifoldFor05s");

            migrationBuilder.CreateTable(
                name: "FoldingFor05s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LayerNumber = table.Column<string>(nullable: true),
                    TestDate = table.Column<DateTime>(nullable: false),
                    SewerManifoldFor05Id = table.Column<Guid>(nullable: false),
                    FillingLaboratoryTestId = table.Column<Guid>(nullable: false),
                    WetDensity = table.Column<double>(nullable: false),
                    MoisturePercentage = table.Column<double>(nullable: false),
                    DryDensity = table.Column<double>(nullable: false),
                    PercentageRequiredCompaction = table.Column<double>(nullable: false),
                    PercentageRealCompaction = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldingFor05s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldingFor05s_FillingLaboratoryTests_FillingLaboratoryTestId",
                        column: x => x.FillingLaboratoryTestId,
                        principalTable: "FillingLaboratoryTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoldingFor05s_SewerManifoldFor05s_SewerManifoldFor05Id",
                        column: x => x.SewerManifoldFor05Id,
                        principalTable: "SewerManifoldFor05s",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldingFor05s_FillingLaboratoryTestId",
                table: "FoldingFor05s",
                column: "FillingLaboratoryTestId");

            migrationBuilder.CreateIndex(
                name: "IX_FoldingFor05s_SewerManifoldFor05Id",
                table: "FoldingFor05s",
                column: "SewerManifoldFor05Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldingFor05s");

            migrationBuilder.AddColumn<double>(
                name: "DryDensity",
                table: "SewerManifoldFor05s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MoisturePercentage",
                table: "SewerManifoldFor05s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageRealCompactio",
                table: "SewerManifoldFor05s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageRequiredCompaction",
                table: "SewerManifoldFor05s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerManifoldFor05s",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "WetDensity",
                table: "SewerManifoldFor05s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_SewerManifoldId",
                table: "SewerManifoldFor05s",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor05s_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor05s",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
