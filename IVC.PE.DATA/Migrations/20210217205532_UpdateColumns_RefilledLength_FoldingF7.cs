using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumns_RefilledLength_FoldingF7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaddedLength",
                table: "FoldingF7s");

            migrationBuilder.AddColumn<double>(
                name: "RefilledLength",
                table: "FoldingF7s",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefilledLength",
                table: "FoldingF7s");

            migrationBuilder.AddColumn<double>(
                name: "PaddedLength",
                table: "FoldingF7s",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
