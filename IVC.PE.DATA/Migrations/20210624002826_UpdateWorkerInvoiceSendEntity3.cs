using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerInvoiceSendEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReceived",
                table: "WorkerInvoiceSends");

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "WorkerInvoiceSends",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollMovementHeaderId",
                table: "WorkerInvoiceSends",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkerInvoiceSends_PayrollMovementHeaderId",
                table: "WorkerInvoiceSends",
                column: "PayrollMovementHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerInvoiceSends_PayrollMovementHeaders_PayrollMovementHeaderId",
                table: "WorkerInvoiceSends",
                column: "PayrollMovementHeaderId",
                principalTable: "PayrollMovementHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerInvoiceSends_PayrollMovementHeaders_PayrollMovementHeaderId",
                table: "WorkerInvoiceSends");

            migrationBuilder.DropIndex(
                name: "IX_WorkerInvoiceSends_PayrollMovementHeaderId",
                table: "WorkerInvoiceSends");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "WorkerInvoiceSends");

            migrationBuilder.DropColumn(
                name: "PayrollMovementHeaderId",
                table: "WorkerInvoiceSends");

            migrationBuilder.AddColumn<bool>(
                name: "isReceived",
                table: "WorkerInvoiceSends",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
