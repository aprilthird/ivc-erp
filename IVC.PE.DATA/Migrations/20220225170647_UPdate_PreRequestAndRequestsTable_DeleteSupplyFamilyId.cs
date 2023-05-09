using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UPdate_PreRequestAndRequestsTable_DeleteSupplyFamilyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRequests_SupplyFamilies_SupplyFamilyId",
                table: "PreRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_SupplyFamilies_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_PreRequests_SupplyFamilyId",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "PreRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "PreRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_SupplyFamilyId",
                table: "PreRequests",
                column: "SupplyFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRequests_SupplyFamilies_SupplyFamilyId",
                table: "PreRequests",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
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
    }
}
