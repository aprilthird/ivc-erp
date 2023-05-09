using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddDocumentTypeId_Procedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "Procedures",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_DocumentTypeId",
                table: "Procedures",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_DocumentTypes_DocumentTypeId",
                table: "Procedures",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_DocumentTypes_DocumentTypeId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_DocumentTypeId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Procedures");
        }
    }
}
