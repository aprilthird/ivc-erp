using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_WorkAreasTable_AddForeignKeyWorkAreaId_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkAreaId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    IntValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAreas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkAreaId",
                table: "AspNetUsers",
                column: "WorkAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkAreas_WorkAreaId",
                table: "AspNetUsers",
                column: "WorkAreaId",
                principalTable: "WorkAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkAreas_WorkAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WorkAreas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkAreaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkAreaId",
                table: "AspNetUsers");
        }
    }
}
