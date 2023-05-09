using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddTechnicalSpecSpecialities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSpecs_Specialities_SpecialityId",
                table: "TechnicalSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSpecs_SpecialityId",
                table: "TechnicalSpecs");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "TechnicalSpecs");

            migrationBuilder.CreateTable(
                name: "TechnicalSpecSpecialities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TechnicalSpecId = table.Column<Guid>(nullable: false),
                    SpecialityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSpecSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecSpecialities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecSpecialities_TechnicalSpecs_TechnicalSpecId",
                        column: x => x.TechnicalSpecId,
                        principalTable: "TechnicalSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecSpecialities_SpecialityId",
                table: "TechnicalSpecSpecialities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecSpecialities_TechnicalSpecId",
                table: "TechnicalSpecSpecialities",
                column: "TechnicalSpecId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicalSpecSpecialities");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialityId",
                table: "TechnicalSpecs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecs_SpecialityId",
                table: "TechnicalSpecs",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSpecs_Specialities_SpecialityId",
                table: "TechnicalSpecs",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
