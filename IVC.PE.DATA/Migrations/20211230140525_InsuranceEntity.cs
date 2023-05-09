using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class InsuranceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InsuranceEntityId",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InsuranceEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceEntity_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachs_InsuranceEntityId",
                table: "EquipmentMachs",
                column: "InsuranceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEntity_ProjectId",
                table: "InsuranceEntity",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachs_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachs",
                column: "InsuranceEntityId",
                principalTable: "InsuranceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachs_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachs");

            migrationBuilder.DropTable(
                name: "InsuranceEntity");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachs_InsuranceEntityId",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "InsuranceEntityId",
                table: "EquipmentMachs");
        }
    }
}
