using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBondAddProjectResponsibleToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BondAddProjectResponsibles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BondAddId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondAddProjectResponsibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BondAddProjectResponsibles_BondAdds_BondAddId",
                        column: x => x.BondAddId,
                        principalTable: "BondAdds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondAddProjectResponsibles_BondAddId",
                table: "BondAddProjectResponsibles",
                column: "BondAddId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BondAddProjectResponsibles");
        }
    }
}
