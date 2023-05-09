using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveIsFromOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFrom",
                table: "EquipmentMachineryOperators");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsFrom",
                table: "EquipmentMachineryOperators",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
