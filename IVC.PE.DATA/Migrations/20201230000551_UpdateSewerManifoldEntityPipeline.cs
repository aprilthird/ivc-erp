using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldEntityPipeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DitchClass",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "LengthOfPipeInstalled",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipeDiameter",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipeType",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<double>(
                name: "LengthOfPipelineInstalled",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PipelineClass",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PipelineDiameter",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PipelineType",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthOfPipelineInstalled",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipelineClass",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipelineDiameter",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipelineType",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<string>(
                name: "DitchClass",
                table: "SewerManifolds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LengthOfPipeInstalled",
                table: "SewerManifolds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PipeDiameter",
                table: "SewerManifolds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PipeType",
                table: "SewerManifolds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
