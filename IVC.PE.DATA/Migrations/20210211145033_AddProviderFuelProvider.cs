using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProviderFuelProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "RUC",
                table: "FuelProviders");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "FuelProviders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FuelProviders_ProviderId",
                table: "FuelProviders",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelProviders_Providers_ProviderId",
                table: "FuelProviders",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelProviders_Providers_ProviderId",
                table: "FuelProviders");

            migrationBuilder.DropIndex(
                name: "IX_FuelProviders_ProviderId",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "FuelProviders");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RUC",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
