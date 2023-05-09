using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_PreRequestFiles_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreRequestFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PreRequestId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequestFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRequestFiles_PreRequests_PreRequestId",
                        column: x => x.PreRequestId,
                        principalTable: "PreRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestFiles_PreRequestId",
                table: "PreRequestFiles",
                column: "PreRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRequestFiles");
        }
    }
}
