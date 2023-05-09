using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SerieNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerieNumber",
                table: "EquipmentProviderFoldings");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentSerie",
                table: "EquipmentMachinerySofts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentSerie",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AddColumn<string>(
                name: "SerieNumber",
                table: "EquipmentProviderFoldings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
