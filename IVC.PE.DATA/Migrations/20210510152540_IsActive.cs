using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class IsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber5",
                table: "Businesses");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfExperiences",
                table: "Professionals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ProfessionalExperienceFoldings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Businesses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive2",
                table: "Businesses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive3",
                table: "Businesses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive4",
                table: "Businesses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive5",
                table: "Businesses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfExperiences",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsActive5",
                table: "Businesses");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address3",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address4",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address5",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber2",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber3",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber4",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber5",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
