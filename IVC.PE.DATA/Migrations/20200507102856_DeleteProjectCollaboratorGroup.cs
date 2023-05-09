using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DeleteProjectCollaboratorGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCollaborators_ProjectCollaboratorGroups_ProjectCollaboratorGroupId",
                table: "ProjectCollaborators");

            migrationBuilder.DropTable(
                name: "ProjectCollaboratorGroups");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCollaborators_ProjectCollaboratorGroupId",
                table: "ProjectCollaborators");

            migrationBuilder.DropColumn(
                name: "ProjectCollaboratorGroupId",
                table: "ProjectCollaborators");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "ProjectCollaborators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCollaborators_ProviderId",
                table: "ProjectCollaborators",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCollaborators_Providers_ProviderId",
                table: "ProjectCollaborators",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCollaborators_Providers_ProviderId",
                table: "ProjectCollaborators");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCollaborators_ProviderId",
                table: "ProjectCollaborators");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "ProjectCollaborators");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCollaboratorGroupId",
                table: "ProjectCollaborators",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectCollaboratorGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RUC = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCollaboratorGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCollaborators_ProjectCollaboratorGroupId",
                table: "ProjectCollaborators",
                column: "ProjectCollaboratorGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCollaborators_ProjectCollaboratorGroups_ProjectCollaboratorGroupId",
                table: "ProjectCollaborators",
                column: "ProjectCollaboratorGroupId",
                principalTable: "ProjectCollaboratorGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
