using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class removeSpecialityProjectFormulaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_ProjectFormulas_ProjectFormulaId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_ProjectFormulaId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "Specialities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "Specialities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_ProjectFormulaId",
                table: "Specialities",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_ProjectFormulas_ProjectFormulaId",
                table: "Specialities",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
