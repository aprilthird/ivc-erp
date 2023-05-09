using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_Stocks_FieldRequests_AddStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockApplicationUsers");

            migrationBuilder.DropTable(
                name: "StockRoofs");

            migrationBuilder.DropTable(
                name: "StockVoucherDetails");

            migrationBuilder.DropTable(
                name: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "QuantityMinimum",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "SalePriceUnit",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Stocks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SupplyEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Measure",
                table: "Stocks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "Stocks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_SupplyId",
                table: "Stocks",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Supplies_SupplyId",
                table: "Stocks",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Supplies_SupplyId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_SupplyId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupplyEntries");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityMinimum",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePriceUnit",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockApplicationUsers_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockRoofs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoofQuantity = table.Column<int>(type: "int", nullable: false),
                    SewerGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockRoofs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockRoofs_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockRoofs_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockRoofs_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickUpResponsible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferencePurchaseOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SewerGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    WasDelivered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockVouchers_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockVouchers_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockVoucherDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SalePriceUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockVoucherDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockVoucherDetails_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockVoucherDetails_StockVouchers_StockVoucherId",
                        column: x => x.StockVoucherId,
                        principalTable: "StockVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockApplicationUsers_StockId",
                table: "StockApplicationUsers",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRoofs_ProjectPhaseId",
                table: "StockRoofs",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRoofs_SewerGroupId",
                table: "StockRoofs",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRoofs_StockId",
                table: "StockRoofs",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVoucherDetails_StockId",
                table: "StockVoucherDetails",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVoucherDetails_StockVoucherId",
                table: "StockVoucherDetails",
                column: "StockVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVouchers_ProjectPhaseId",
                table: "StockVouchers",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVouchers_SewerGroupId",
                table: "StockVouchers",
                column: "SewerGroupId");
        }
    }
}
