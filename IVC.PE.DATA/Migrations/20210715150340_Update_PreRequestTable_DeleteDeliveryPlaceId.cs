using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_PreRequestTable_DeleteDeliveryPlaceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRequests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "PreRequests");

            migrationBuilder.DropIndex(
                name: "IX_PreRequests_RequestDeliveryPlaceId",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "RequestDeliveryPlaceId",
                table: "PreRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestDeliveryPlaceId",
                table: "PreRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_RequestDeliveryPlaceId",
                table: "PreRequests",
                column: "RequestDeliveryPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRequests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "PreRequests",
                column: "RequestDeliveryPlaceId",
                principalTable: "RequestDeliveryPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
