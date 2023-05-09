using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddThoricalLayerColumn_SewerManifoldFor05_Update_SewermanifoldFor24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProviderId",
                table: "SewerManifoldFor24FirstParts",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "TheoreticalLayer",
                table: "SewerManifoldFor05s",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TheoreticalLayer",
                table: "SewerManifoldFor05s");

            migrationBuilder.AlterColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProviderId",
                table: "SewerManifoldFor24FirstParts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
