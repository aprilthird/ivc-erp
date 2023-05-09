using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BusinessConsortium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessConsortium1",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessConsortium2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessConsortium3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessConsortium4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessConsortium5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation1",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConsortiumParticipation5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DefaultParticipation",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Businesses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessConsortium1",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BusinessConsortium2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BusinessConsortium3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BusinessConsortium4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BusinessConsortium5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation1",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ConsortiumParticipation5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "DefaultParticipation",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Businesses");
        }
    }
}
