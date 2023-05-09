using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fix_column_UserOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentCertificateUserOperators");

            migrationBuilder.AddColumn<string>(
                name: "Operator",
                table: "EquipmentCertificateUserOperators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operator",
                table: "EquipmentCertificateUserOperators");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentCertificateUserOperators",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
