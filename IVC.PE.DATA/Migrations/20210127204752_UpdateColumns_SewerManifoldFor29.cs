using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumns_SewerManifoldFor29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlphaltType",
                table: "SewerManifoldFor29s");

            migrationBuilder.AlterColumn<int>(
                name: "Thickness",
                table: "SewerManifoldFor29s",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "AsphaltType",
                table: "SewerManifoldFor29s",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsphaltType",
                table: "SewerManifoldFor29s");

            migrationBuilder.AlterColumn<double>(
                name: "Thickness",
                table: "SewerManifoldFor29s",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AlphaltType",
                table: "SewerManifoldFor29s",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
