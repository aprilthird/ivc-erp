using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddTechnicalSpec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechnicalSpecs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false),
                    SpecialityId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecs_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecs_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecs_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecs_SpecialityId",
                table: "TechnicalSpecs",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecs_SupplyFamilyId",
                table: "TechnicalSpecs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecs_SupplyGroupId",
                table: "TechnicalSpecs",
                column: "SupplyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicalSpecs");
        }
    }
}
