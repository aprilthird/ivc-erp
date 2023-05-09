using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewRoleSystem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkAreaItem_WorkAreaItem_ParentId",
                table: "WorkAreaItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAreaItem_WorkAreas_WorkAreaId",
                table: "WorkAreaItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkAreaItem",
                table: "WorkAreaItem");

            migrationBuilder.RenameTable(
                name: "WorkAreaItem",
                newName: "WorkAreaItems");

            migrationBuilder.RenameIndex(
                name: "IX_WorkAreaItem_WorkAreaId",
                table: "WorkAreaItems",
                newName: "IX_WorkAreaItems_WorkAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkAreaItem_ParentId",
                table: "WorkAreaItems",
                newName: "IX_WorkAreaItems_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkAreaItems",
                table: "WorkAreaItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorkRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkRoleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkRoleId = table.Column<Guid>(nullable: false),
                    WorkAreaItemId = table.Column<Guid>(nullable: false),
                    PermissionLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkRoleItems_WorkAreaItems_WorkAreaItemId",
                        column: x => x.WorkAreaItemId,
                        principalTable: "WorkAreaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkRoleItems_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoleItems_WorkAreaItemId",
                table: "WorkRoleItems",
                column: "WorkAreaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoleItems_WorkRoleId",
                table: "WorkRoleItems",
                column: "WorkRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAreaItems_WorkAreaItems_ParentId",
                table: "WorkAreaItems",
                column: "ParentId",
                principalTable: "WorkAreaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAreaItems_WorkAreas_WorkAreaId",
                table: "WorkAreaItems",
                column: "WorkAreaId",
                principalTable: "WorkAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkAreaItems_WorkAreaItems_ParentId",
                table: "WorkAreaItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAreaItems_WorkAreas_WorkAreaId",
                table: "WorkAreaItems");

            migrationBuilder.DropTable(
                name: "WorkRoleItems");

            migrationBuilder.DropTable(
                name: "WorkRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkAreaItems",
                table: "WorkAreaItems");

            migrationBuilder.RenameTable(
                name: "WorkAreaItems",
                newName: "WorkAreaItem");

            migrationBuilder.RenameIndex(
                name: "IX_WorkAreaItems_WorkAreaId",
                table: "WorkAreaItem",
                newName: "IX_WorkAreaItem_WorkAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkAreaItems_ParentId",
                table: "WorkAreaItem",
                newName: "IX_WorkAreaItem_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkAreaItem",
                table: "WorkAreaItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAreaItem_WorkAreaItem_ParentId",
                table: "WorkAreaItem",
                column: "ParentId",
                principalTable: "WorkAreaItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAreaItem_WorkAreas_WorkAreaId",
                table: "WorkAreaItem",
                column: "WorkAreaId",
                principalTable: "WorkAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
