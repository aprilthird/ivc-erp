using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelPartFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryFuelTransportParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportPartId = table.Column<Guid>(nullable: false),
                    AcumulatedMileage = table.Column<int>(nullable: false),
                    AcumulatedGallon = table.Column<int>(nullable: false),
                    RateConsume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryFuelTransportParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportParts_EquipmentMachineryTransportParts_EquipmentMachineryTransportPartId",
                        column: x => x.EquipmentMachineryTransportPartId,
                        principalTable: "EquipmentMachineryTransportParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                        column: x => x.EquipmentMachineryTypeId,
                        principalTable: "EquipmentMachineryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportParts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryFuelTransportPartFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryFuelTransportPartId = table.Column<Guid>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    FuelProviderId = table.Column<Guid>(nullable: false),
                    PartDate = table.Column<DateTime>(nullable: false),
                    PartHour = table.Column<string>(nullable: true),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    Mileage = table.Column<int>(nullable: false),
                    Gallon = table.Column<int>(nullable: false),
                    Hour = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryFuelTransportPartFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportPartFoldings_EquipmentMachineryFuelTransportParts_EquipmentMachineryFuelTransportPartId",
                        column: x => x.EquipmentMachineryFuelTransportPartId,
                        principalTable: "EquipmentMachineryFuelTransportParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportPartFoldings_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryFuelTransportPartFoldings_FuelProviders_FuelProviderId",
                        column: x => x.FuelProviderId,
                        principalTable: "FuelProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_EquipmentMachineryFuelTransportPartId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "EquipmentMachineryFuelTransportPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_EquipmentMachineryOperatorId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_FuelProviderId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "FuelProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_EquipmentMachineryTransportPartId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "EquipmentMachineryTransportPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_EquipmentProviderId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "EquipmentProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryFuelTransportParts");
        }
    }
}
