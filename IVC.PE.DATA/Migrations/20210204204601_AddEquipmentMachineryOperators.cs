using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEquipmentMachineryOperators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryOperators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    PhoneOperator = table.Column<string>(nullable: true),
                    HiringType = table.Column<int>(nullable: false),
                    ConditionalType = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: true),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: true),
                    Categorytype = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeTypeId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryOperators");
        }
    }
}
