using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UserAndEmployees2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PensionFundAdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionFundAdministratorId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PensionFundAdministratorId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PensionFundAdministratorId",
                table: "AspNetUsers",
                column: "PensionFundAdministratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "AspNetUsers",
                column: "PensionFundAdministratorId",
                principalTable: "PensionFundAdministrators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
