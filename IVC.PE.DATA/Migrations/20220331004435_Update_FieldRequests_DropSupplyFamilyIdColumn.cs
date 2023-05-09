using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_FieldRequests_DropSupplyFamilyIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldRequests_SupplyFamilies_SupplyFamilyId",
                table: "FieldRequests");

            migrationBuilder.DropIndex(
                name: "IX_FieldRequests_SupplyFamilyId",
                table: "FieldRequests");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "FieldRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "FieldRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_SupplyFamilyId",
                table: "FieldRequests",
                column: "SupplyFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRequests_SupplyFamilies_SupplyFamilyId",
                table: "FieldRequests",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
