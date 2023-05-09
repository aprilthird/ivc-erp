using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelMachPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryFuelMachParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachPartId = table.Column<Guid>(nullable: false),
                    AcumulatedGallon = table.Column<int>(nullable: false),
                    FoldingNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryFuelMachParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachParts_EquipmentMachParts_EquipmentMachPartId",
                        column: x => x.EquipmentMachPartId,
                        principalTable: "EquipmentMachParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachParts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryFuelMachPartFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryFuelMachPartId = table.Column<Guid>(nullable: false),
                    FuelProviderFoldingId = table.Column<Guid>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    FuelProviderId = table.Column<Guid>(nullable: false),
                    PartDate = table.Column<DateTime>(nullable: false),
                    PartHour = table.Column<string>(nullable: true),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    Horometer = table.Column<int>(nullable: false),
                    Gallon = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryFuelMachPartFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachPartFoldings_EquipmentMachineryFuelMachParts_EquipmentMachineryFuelMachPartId",
                        column: x => x.EquipmentMachineryFuelMachPartId,
                        principalTable: "EquipmentMachineryFuelMachParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachPartFoldings_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachPartFoldings_FuelProviderFoldings_FuelProviderFoldingId",
                        column: x => x.FuelProviderFoldingId,
                        principalTable: "FuelProviderFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelMachPartFoldings_FuelProviders_FuelProviderId",
                        column: x => x.FuelProviderId,
                        principalTable: "FuelProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_EquipmentMachineryFuelMachPartId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "EquipmentMachineryFuelMachPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_EquipmentMachineryOperatorId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_FuelProviderFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "FuelProviderFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_FuelProviderId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "FuelProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachParts_EquipmentMachPartId",
                table: "EquipmentMachineryFuelMachParts",
                column: "EquipmentMachPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachParts_EquipmentProviderId",
                table: "EquipmentMachineryFuelMachParts",
                column: "EquipmentProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryFuelMachPartFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryFuelMachParts");
        }
    }
}
