using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrdersTable_AddGlosaAndOtherDescription_RemoveDeliveryRequestPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestDeliveryPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "OtherDescription",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Glosa",
                table: "OrderItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherDescription",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Glosa",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestDeliveryPlaceId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestDeliveryPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDeliveryPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestDeliveryPlaces_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestDeliveryPlaceId",
                table: "Requests",
                column: "RequestDeliveryPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDeliveryPlaces_ProjectId",
                table: "RequestDeliveryPlaces",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "Requests",
                column: "RequestDeliveryPlaceId",
                principalTable: "RequestDeliveryPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
