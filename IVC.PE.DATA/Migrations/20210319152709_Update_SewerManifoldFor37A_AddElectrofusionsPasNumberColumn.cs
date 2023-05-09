using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SewerManifoldFor37A_AddElectrofusionsPasNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectrofusionsPasNumber",
                table: "SewerManifoldFor37As",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectrofusionsPasNumber",
                table: "SewerManifoldFor37As");
        }
    }
}
