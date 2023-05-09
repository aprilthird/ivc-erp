using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkerNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Workgroup",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InterestGroupEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InterestGroupId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestGroupEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestGroupEmails_InterestGroups_InterestGroupId",
                        column: x => x.InterestGroupId,
                        principalTable: "InterestGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestGroupEmails_InterestGroupId",
                table: "InterestGroupEmails",
                column: "InterestGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestGroupEmails");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Workgroup",
                table: "Workers");
        }
    }
}
