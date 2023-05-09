using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondsModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BondRenovations");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "BondRenovations");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BondRenovations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
