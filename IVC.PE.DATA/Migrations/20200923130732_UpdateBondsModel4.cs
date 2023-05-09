using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondsModel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "BondRenovations");

            migrationBuilder.AddColumn<int>(
                name: "BondOrder",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BondOrder",
                table: "BondRenovations");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "BondRenovations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
