using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBiddingWorkComponentDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiddingWorkComponentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BiddingWorkId = table.Column<Guid>(nullable: false),
                    BiddingWorkComponentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingWorkComponentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiddingWorkComponentDetails_BiddingWorkComponents_BiddingWorkComponentId",
                        column: x => x.BiddingWorkComponentId,
                        principalTable: "BiddingWorkComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BiddingWorkComponentDetails_BiddingWorks_BiddingWorkId",
                        column: x => x.BiddingWorkId,
                        principalTable: "BiddingWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorkComponentDetails_BiddingWorkComponentId",
                table: "BiddingWorkComponentDetails",
                column: "BiddingWorkComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorkComponentDetails_BiddingWorkId",
                table: "BiddingWorkComponentDetails",
                column: "BiddingWorkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiddingWorkComponentDetails");
        }
    }
}
