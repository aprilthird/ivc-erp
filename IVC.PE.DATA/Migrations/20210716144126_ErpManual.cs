using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ErpManual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErpManuals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AreaModuleId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErpManuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErpManuals_AreaModules_AreaModuleId",
                        column: x => x.AreaModuleId,
                        principalTable: "AreaModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErpManuals_AreaModuleId",
                table: "ErpManuals",
                column: "AreaModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErpManuals");
        }
    }
}
