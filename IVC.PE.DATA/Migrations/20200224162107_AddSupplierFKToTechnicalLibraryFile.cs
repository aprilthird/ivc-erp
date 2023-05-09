using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSupplierFKToTechnicalLibraryFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "TechnicalLibraryFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalLibraryFiles_SupplierId",
                table: "TechnicalLibraryFiles",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalLibraryFiles_Suppliers_SupplierId",
                table: "TechnicalLibraryFiles",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalLibraryFiles_Suppliers_SupplierId",
                table: "TechnicalLibraryFiles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalLibraryFiles_SupplierId",
                table: "TechnicalLibraryFiles");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "TechnicalLibraryFiles");
        }
    }
}
