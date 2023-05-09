using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnsVideoTestDischargeManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<string>(
                name: "MirrorTestVideoUrl",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MonkeyBallTestVideoUrl",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoomTestVideoUrl",
                table: "DischargeManifolds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MirrorTestVideoUrl",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "MonkeyBallTestVideoUrl",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "ZoomTestVideoUrl",
                table: "DischargeManifolds");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "DischargeManifolds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
