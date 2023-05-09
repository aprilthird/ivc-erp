using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterInterestGroups_InterestGroup_InterestGroupId",
                table: "LetterInterestGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestGroup",
                table: "InterestGroup");

            migrationBuilder.RenameTable(
                name: "InterestGroup",
                newName: "InterestGroups");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseTerm",
                table: "Letters",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Letters",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestGroups",
                table: "InterestGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterInterestGroups_InterestGroups_InterestGroupId",
                table: "LetterInterestGroups",
                column: "InterestGroupId",
                principalTable: "InterestGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterInterestGroups_InterestGroups_InterestGroupId",
                table: "LetterInterestGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestGroups",
                table: "InterestGroups");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Letters");

            migrationBuilder.RenameTable(
                name: "InterestGroups",
                newName: "InterestGroup");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseTerm",
                table: "Letters",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestGroup",
                table: "InterestGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterInterestGroups_InterestGroup_InterestGroupId",
                table: "LetterInterestGroups",
                column: "InterestGroupId",
                principalTable: "InterestGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
