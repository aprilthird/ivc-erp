using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateStockVoucherModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "StockVouchers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUpResponsible",
                table: "StockVouchers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "StockVouchers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "StockVouchers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasDelivered",
                table: "StockVouchers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StockVouchers_ProjectPhaseId",
                table: "StockVouchers",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVouchers_SewerGroupId",
                table: "StockVouchers",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockVouchers_ProjectPhases_ProjectPhaseId",
                table: "StockVouchers",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockVouchers_SewerGroups_SewerGroupId",
                table: "StockVouchers",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockVouchers_ProjectPhases_ProjectPhaseId",
                table: "StockVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockVouchers_SewerGroups_SewerGroupId",
                table: "StockVouchers");

            migrationBuilder.DropIndex(
                name: "IX_StockVouchers_ProjectPhaseId",
                table: "StockVouchers");

            migrationBuilder.DropIndex(
                name: "IX_StockVouchers_SewerGroupId",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "PickUpResponsible",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "WasDelivered",
                table: "StockVouchers");
        }
    }
}
