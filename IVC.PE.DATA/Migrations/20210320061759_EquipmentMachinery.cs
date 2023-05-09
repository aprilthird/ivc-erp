using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachinery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentProviderFoldingId = table.Column<Guid>(nullable: false),
                    MachineryName = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Potency = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    SerieNumber = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Cubing = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ServiceCondition = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    InsuranceNumber = table.Column<int>(nullable: false),
                    LastStartDateInsurance = table.Column<DateTime>(nullable: true),
                    LastEndDateInsurance = table.Column<DateTime>(nullable: true),
                    LastValidityInsurance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineries_EquipmentProviderFoldings_EquipmentProviderFoldingId",
                        column: x => x.EquipmentProviderFoldingId,
                        principalTable: "EquipmentProviderFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineries_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryInsuranceFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryId = table.Column<Guid>(nullable: false),
                    Validity = table.Column<int>(nullable: false),
                    StartDateInsurance = table.Column<DateTime>(nullable: true),
                    EndDateInsurance = table.Column<DateTime>(nullable: true),
                    InsuranceFileUrl = table.Column<string>(nullable: true),
                    OrderInsurance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryInsuranceFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryInsuranceFoldings_EquipmentMachineries_EquipmentMachineryId",
                        column: x => x.EquipmentMachineryId,
                        principalTable: "EquipmentMachineries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineries_EquipmentProviderFoldingId",
                table: "EquipmentMachineries",
                column: "EquipmentProviderFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineries_EquipmentProviderId",
                table: "EquipmentMachineries",
                column: "EquipmentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineries_ProjectId",
                table: "EquipmentMachineries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryInsuranceFoldings_EquipmentMachineryId",
                table: "EquipmentMachineryInsuranceFoldings",
                column: "EquipmentMachineryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryInsuranceFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineries");
        }
    }
}
