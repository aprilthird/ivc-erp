using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnsPipeBatchDischargeManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForthPipeBatch",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThridPipeBatch",
                table: "DischargeManifolds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForthPipeBatch",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "ThridPipeBatch",
                table: "DischargeManifolds");
        }
    }
}
