using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntriesTableName_Create_SupplyEntryItems_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuppliyEntries_Orders_OrderId",
                table: "SuppliyEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SuppliyEntries_Providers_ProviderId",
                table: "SuppliyEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuppliyEntries",
                table: "SuppliyEntries");

            migrationBuilder.RenameTable(
                name: "SuppliyEntries",
                newName: "SupplyEntries");

            migrationBuilder.RenameIndex(
                name: "IX_SuppliyEntries_ProviderId",
                table: "SupplyEntries",
                newName: "IX_SupplyEntries_ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_SuppliyEntries_OrderId",
                table: "SupplyEntries",
                newName: "IX_SupplyEntries_OrderId");

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAttention",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplyEntries",
                table: "SupplyEntries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SupplyEntryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplyEntryId = table.Column<Guid>(nullable: false),
                    Measure = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyEntryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyEntryItems_SupplyEntries_SupplyEntryId",
                        column: x => x.SupplyEntryId,
                        principalTable: "SupplyEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyEntries_WarehouseId",
                table: "SupplyEntries",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyEntryItems_SupplyEntryId",
                table: "SupplyEntryItems",
                column: "SupplyEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntries_Orders_OrderId",
                table: "SupplyEntries",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntries_Providers_ProviderId",
                table: "SupplyEntries",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntries_Warehouses_WarehouseId",
                table: "SupplyEntries",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntries_Orders_OrderId",
                table: "SupplyEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntries_Providers_ProviderId",
                table: "SupplyEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntries_Warehouses_WarehouseId",
                table: "SupplyEntries");

            migrationBuilder.DropTable(
                name: "SupplyEntryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplyEntries",
                table: "SupplyEntries");

            migrationBuilder.DropIndex(
                name: "IX_SupplyEntries_WarehouseId",
                table: "SupplyEntries");

            migrationBuilder.DropColumn(
                name: "MeasureInAttention",
                table: "OrderItems");

            migrationBuilder.RenameTable(
                name: "SupplyEntries",
                newName: "SuppliyEntries");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyEntries_ProviderId",
                table: "SuppliyEntries",
                newName: "IX_SuppliyEntries_ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyEntries_OrderId",
                table: "SuppliyEntries",
                newName: "IX_SuppliyEntries_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuppliyEntries",
                table: "SuppliyEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliyEntries_Orders_OrderId",
                table: "SuppliyEntries",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliyEntries_Providers_ProviderId",
                table: "SuppliyEntries",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
