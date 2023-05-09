using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRequestDeliveryPlaceAddProjectProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "RequestDeliveryPlaces",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RequestDeliveryPlaces_ProjectId",
                table: "RequestDeliveryPlaces",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeliveryPlaces_Projects_ProjectId",
                table: "RequestDeliveryPlaces",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeliveryPlaces_Projects_ProjectId",
                table: "RequestDeliveryPlaces");

            migrationBuilder.DropIndex(
                name: "IX_RequestDeliveryPlaces_ProjectId",
                table: "RequestDeliveryPlaces");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "RequestDeliveryPlaces");
        }
    }
}
