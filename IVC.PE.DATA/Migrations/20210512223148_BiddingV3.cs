using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BiddingV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodeNumber",
                table: "Businesses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CodeNumber",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ContractAmmount",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IvcParticipation",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LiquidationAmmount",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ParticipationAmmount",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeNumber",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "CodeNumber",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "ContractAmmount",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "IvcParticipation",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "LiquidationAmmount",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "ParticipationAmmount",
                table: "BiddingWorks");
        }
    }
}
