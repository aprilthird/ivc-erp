using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Transports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTypeTransports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTypeTransports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTypeTransportActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTransportId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTypeTransportActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTypeTransportActivities_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                        column: x => x.EquipmentMachineryTypeTransportId,
                        principalTable: "EquipmentMachineryTypeTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTypeTransportActivities_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryTypeTransportActivities",
                column: "EquipmentMachineryTypeTransportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryTypeTransportActivities");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTypeTransports");
        }
    }
}
