using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BusinessParticipationFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation1",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation5",
                table: "Businesses");

            migrationBuilder.CreateTable(
                name: "BusinessParticipationFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessId = table.Column<Guid>(nullable: false),
                    IvcParticipation = table.Column<double>(nullable: false),
                    TestimonyUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessParticipationFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessParticipationFoldings_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessParticipationFoldings_BusinessId",
                table: "BusinessParticipationFoldings",
                column: "BusinessId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessParticipationFoldings");

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation1",
                table: "Businesses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation2",
                table: "Businesses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation3",
                table: "Businesses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation4",
                table: "Businesses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation5",
                table: "Businesses",
                type: "float",
                nullable: true);
        }
    }
}
