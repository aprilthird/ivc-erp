using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddUsersColumnsNewSIGProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NewSIGProcesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "NewSIGProcesses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NewSIGProcesses");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "NewSIGProcesses");
        }
    }
}
