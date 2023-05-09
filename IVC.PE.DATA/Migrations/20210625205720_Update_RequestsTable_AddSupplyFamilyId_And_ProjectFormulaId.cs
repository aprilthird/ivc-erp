using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestsTable_AddSupplyFamilyId_And_ProjectFormulaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProjectFormulaId",
                table: "Requests",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests",
                column: "SupplyFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ProjectFormulas_ProjectFormulaId",
                table: "Requests",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_SupplyFamilies_SupplyFamilyId",
                table: "Requests",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ProjectFormulas_ProjectFormulaId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_SupplyFamilies_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ProjectFormulaId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "Requests");
        }
    }
}
