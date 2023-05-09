using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewRoleSystem1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "WorkAreas",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkAreas",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NewRoleSystem",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PermissionLevel",
                table: "AspNetUserRoles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkAreaItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: false),
                    WorkAreaId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ApplicationRoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAreaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkAreaItem_WorkAreaItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "WorkAreaItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkAreaItem_WorkAreas_WorkAreaId",
                        column: x => x.WorkAreaId,
                        principalTable: "WorkAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkAreaItem_ParentId",
                table: "WorkAreaItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAreaItem_WorkAreaId",
                table: "WorkAreaItem",
                column: "WorkAreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkAreaItem");

            migrationBuilder.DropColumn(
                name: "NewRoleSystem",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermissionLevel",
                table: "AspNetUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "WorkAreas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkAreas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
