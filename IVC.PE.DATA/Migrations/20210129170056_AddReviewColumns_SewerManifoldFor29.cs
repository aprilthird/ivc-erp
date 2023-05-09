using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddReviewColumns_SewerManifoldFor29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Pavement2InReview",
                table: "SewerManifoldFor29s",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pavement3InMixedReview",
                table: "SewerManifoldFor29s",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pavement3InReview",
                table: "SewerManifoldFor29s",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pavement2InReview",
                table: "SewerManifoldFor29s");

            migrationBuilder.DropColumn(
                name: "Pavement3InMixedReview",
                table: "SewerManifoldFor29s");

            migrationBuilder.DropColumn(
                name: "Pavement3InReview",
                table: "SewerManifoldFor29s");
        }
    }
}
