using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class change6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BondAdds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_ProjectId",
                table: "BondAdds",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_Projects_ProjectId",
                table: "BondAdds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_Projects_ProjectId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_ProjectId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BondAdds");
        }
    }
}
