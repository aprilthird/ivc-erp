using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRdpItemModule5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RdpItemAccumulatedAmmounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RdpItemId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    AccumulatedAmmount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdpItemAccumulatedAmmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdpItemAccumulatedAmmounts_RdpItems_RdpItemId",
                        column: x => x.RdpItemId,
                        principalTable: "RdpItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpItemAccumulatedAmmounts_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemAccumulatedAmmounts_RdpItemId",
                table: "RdpItemAccumulatedAmmounts",
                column: "RdpItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemAccumulatedAmmounts_SewerGroupId",
                table: "RdpItemAccumulatedAmmounts",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RdpItemAccumulatedAmmounts");
        }
    }
}
