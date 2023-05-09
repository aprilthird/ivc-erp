using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class deleteproyectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.DropForeignKey(
                name: "FK_BondLoads_Projects_ProjectId",
                table: "BondLoads");

            migrationBuilder.DropIndex(
                name: "IX_BondLoads_ProjectId",
                table: "BondLoads");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BondLoads");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BondLoads",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_ProjectId",
                table: "BondLoads",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondLoads_Projects_ProjectId",
                table: "BondLoads",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }
    }
}
