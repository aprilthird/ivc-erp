using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class businessfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessFiles_Providers_ProviderId",
                table: "BusinessFiles");

            migrationBuilder.DropIndex(
                name: "IX_BusinessFiles_ProviderId",
                table: "BusinessFiles");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "BusinessFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessId",
                table: "BusinessFiles",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessId",
                table: "BusinessFiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "BusinessFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BusinessFiles_ProviderId",
                table: "BusinessFiles",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessFiles_Providers_ProviderId",
                table: "BusinessFiles",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
