using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewRoleSystem3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "WorkAreaItems");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "WorkAreaItems",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkAreaItems_RoleId",
            //    table: "WorkAreaItems",
            //    column: "RoleId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_WorkAreaItems_AspNetRoles_RoleId",
            //    table: "WorkAreaItems",
            //    column: "RoleId",
            //    principalTable: "AspNetRoles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_WorkAreaItems_AspNetRoles_RoleId",
            //    table: "WorkAreaItems");

            //migrationBuilder.DropIndex(
            //    name: "IX_WorkAreaItems_RoleId",
            //    table: "WorkAreaItems");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "WorkAreaItems");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRoleId",
                table: "WorkAreaItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
