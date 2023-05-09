using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRequestAuthsEntityAddRequestAndUserTypeProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedDate",
                table: "RequestAuthorizations",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestId",
                table: "RequestAuthorizations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "RequestAuthorizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestAuthorizations_RequestId",
                table: "RequestAuthorizations",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAuthorizations_Requests_RequestId",
                table: "RequestAuthorizations",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAuthorizations_Requests_RequestId",
                table: "RequestAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_RequestAuthorizations_RequestId",
                table: "RequestAuthorizations");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "RequestAuthorizations");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "RequestAuthorizations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedDate",
                table: "RequestAuthorizations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
