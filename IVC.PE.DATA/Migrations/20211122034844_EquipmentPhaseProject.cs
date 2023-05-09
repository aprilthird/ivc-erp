using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentPhaseProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TransportPhases",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "MachineryPhases",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportPhases_ProjectId",
                table: "TransportPhases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineryPhases_ProjectId",
                table: "MachineryPhases",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineryPhases_Projects_ProjectId",
                table: "MachineryPhases",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportPhases_Projects_ProjectId",
                table: "TransportPhases",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineryPhases_Projects_ProjectId",
                table: "MachineryPhases");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportPhases_Projects_ProjectId",
                table: "TransportPhases");

            migrationBuilder.DropIndex(
                name: "IX_TransportPhases_ProjectId",
                table: "TransportPhases");

            migrationBuilder.DropIndex(
                name: "IX_MachineryPhases_ProjectId",
                table: "MachineryPhases");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TransportPhases");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "MachineryPhases");
        }
    }
}
