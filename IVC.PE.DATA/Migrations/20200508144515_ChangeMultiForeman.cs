using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ChangeMultiForeman : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId",
                unique: true,
                filter: "[ForemanId] IS NOT NULL");
        }
    }
}
