using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondsModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BondAddId",
                table: "BondRenovations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRenovations",
                table: "BondAdds",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BondRenovations_BondAddId",
                table: "BondRenovations",
                column: "BondAddId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondRenovations_BondAdds_BondAddId",
                table: "BondRenovations",
                column: "BondAddId",
                principalTable: "BondAdds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondRenovations_BondAdds_BondAddId",
                table: "BondRenovations");

            migrationBuilder.DropIndex(
                name: "IX_BondRenovations_BondAddId",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "BondAddId",
                table: "BondRenovations");

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfRenovations",
                table: "BondAdds",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
