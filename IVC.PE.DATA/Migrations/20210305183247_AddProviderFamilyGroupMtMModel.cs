using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProviderFamilyGroupMtMModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderSupplyFamilies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProviderId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSupplyFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderSupplyFamilies_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderSupplyFamilies_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderSupplyGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProviderId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSupplyGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderSupplyGroups_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderSupplyGroups_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSupplyFamilies_ProviderId",
                table: "ProviderSupplyFamilies",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSupplyFamilies_SupplyFamilyId",
                table: "ProviderSupplyFamilies",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSupplyGroups_ProviderId",
                table: "ProviderSupplyGroups",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSupplyGroups_SupplyGroupId",
                table: "ProviderSupplyGroups",
                column: "SupplyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderSupplyFamilies");

            migrationBuilder.DropTable(
                name: "ProviderSupplyGroups");
        }
    }
}
