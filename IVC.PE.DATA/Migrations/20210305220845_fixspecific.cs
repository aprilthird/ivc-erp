using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixspecific : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "specific",
                table: "EquipmentMachinerySoftPartPlusUltra",
                newName: "Specific");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specific",
                table: "EquipmentMachinerySoftPartPlusUltra",
                newName: "specific");
        }
    }
}
