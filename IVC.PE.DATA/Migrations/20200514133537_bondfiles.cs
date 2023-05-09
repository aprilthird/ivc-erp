using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class bondfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BondFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    BondAddId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BondFiles_BondAdds_BondAddId",
                        column: x => x.BondAddId,
                        principalTable: "BondAdds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondFiles_BondAddId",
                table: "BondFiles",
                column: "BondAddId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BondFiles");
        }
    }
}
