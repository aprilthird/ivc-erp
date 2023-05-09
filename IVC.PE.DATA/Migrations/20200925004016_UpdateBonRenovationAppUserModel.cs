using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBonRenovationAppUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BondRenovationApplicationUsers_BondRenovationId",
                table: "BondRenovationApplicationUsers",
                column: "BondRenovationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondRenovationApplicationUsers_BondRenovations_BondRenovationId",
                table: "BondRenovationApplicationUsers",
                column: "BondRenovationId",
                principalTable: "BondRenovations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondRenovationApplicationUsers_BondRenovations_BondRenovationId",
                table: "BondRenovationApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_BondRenovationApplicationUsers_BondRenovationId",
                table: "BondRenovationApplicationUsers");
        }
    }
}
