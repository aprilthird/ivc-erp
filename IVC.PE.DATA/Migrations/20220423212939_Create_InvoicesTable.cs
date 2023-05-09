using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_InvoicesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "SupplyEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Serie = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyEntries_InvoiceId",
                table: "SupplyEntries",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntries_Invoices_InvoiceId",
                table: "SupplyEntries",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntries_Invoices_InvoiceId",
                table: "SupplyEntries");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_SupplyEntries_InvoiceId",
                table: "SupplyEntries");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "SupplyEntries");
        }
    }
}
