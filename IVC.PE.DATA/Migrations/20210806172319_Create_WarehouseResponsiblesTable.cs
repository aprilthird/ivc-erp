using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_WarehouseResponsiblesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WarehouseTypes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WarehouseResponsibles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseResponsibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseResponsibles_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTypes_ProjectId",
                table: "WarehouseTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseResponsibles_WarehouseId",
                table: "WarehouseResponsibles",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTypes_Projects_ProjectId",
                table: "WarehouseTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTypes_Projects_ProjectId",
                table: "WarehouseTypes");

            migrationBuilder.DropTable(
                name: "WarehouseResponsibles");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTypes_ProjectId",
                table: "WarehouseTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WarehouseTypes");
        }
    }
}
