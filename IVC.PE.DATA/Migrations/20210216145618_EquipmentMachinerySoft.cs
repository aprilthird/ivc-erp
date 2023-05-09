using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachinerySoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySofts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentName = table.Column<string>(nullable: true),
                    EquipmentYear = table.Column<string>(nullable: true),
                    EquipmentPlate = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EquipmentMachineryAssignedUserId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ServiceCondition = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    StartDateInsurance = table.Column<DateTime>(nullable: false),
                    EndDateInsurance = table.Column<DateTime>(nullable: false),
                    InsuranceFileUrl = table.Column<string>(nullable: true),
                    StartDateSOAT = table.Column<DateTime>(nullable: false),
                    EndDateSOAT = table.Column<DateTime>(nullable: false),
                    InsuranceSOATFileUrl = table.Column<string>(nullable: true),
                    StartDateTechnicalRevision = table.Column<DateTime>(nullable: false),
                    EndDateTechnicalRevision = table.Column<DateTime>(nullable: false),
                    InsuranceTechnicalRevisionFileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySofts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySofts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                        column: x => x.EquipmentMachineryAssignedUserId,
                        principalTable: "EquipmentMachineryAssignedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySofts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentMachineryAssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentProviderId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachinerySofts");
        }
    }
}
