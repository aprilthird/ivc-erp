using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorMixDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignTypeId",
                table: "MixDesigns",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_DesignTypeId",
                table: "MixDesigns",
                column: "DesignTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDesigns_DesignTypes_DesignTypeId",
                table: "MixDesigns",
                column: "DesignTypeId",
                principalTable: "DesignTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDesigns_DesignTypes_DesignTypeId",
                table: "MixDesigns");

            migrationBuilder.DropIndex(
                name: "IX_MixDesigns_DesignTypeId",
                table: "MixDesigns");

            migrationBuilder.DropColumn(
                name: "DesignTypeId",
                table: "MixDesigns");
        }
    }
}
