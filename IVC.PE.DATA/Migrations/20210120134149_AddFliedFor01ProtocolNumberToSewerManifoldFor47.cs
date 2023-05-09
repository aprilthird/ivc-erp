using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddFliedFor01ProtocolNumberToSewerManifoldFor47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "For01ProtocolNumber",
                table: "SewerManifoldFor47s",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "For01ProtocolNumber",
                table: "SewerManifoldFor47s");
        }
    }
}
