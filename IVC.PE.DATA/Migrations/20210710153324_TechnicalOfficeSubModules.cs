using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TechnicalOfficeSubModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MixDesigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CementTypeId = table.Column<Guid>(nullable: false),
                    AggregateTypeId = table.Column<Guid>(nullable: false),
                    ConcreteUseId = table.Column<Guid>(nullable: false),
                    Additive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DesignDate = table.Column<DateTime>(nullable: false),
                    TechnicalVersionId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDesigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDesigns_AggregateTypes_AggregateTypeId",
                        column: x => x.AggregateTypeId,
                        principalTable: "AggregateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MixDesigns_CementTypes_CementTypeId",
                        column: x => x.CementTypeId,
                        principalTable: "CementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MixDesigns_ConcreteUses_ConcreteUseId",
                        column: x => x.ConcreteUseId,
                        principalTable: "ConcreteUses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MixDesigns_TechnicalVersions_TechnicalVersionId",
                        column: x => x.TechnicalVersionId,
                        principalTable: "TechnicalVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderCatalogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false),
                    SpecialityId = table.Column<Guid>(nullable: true),
                    ProviderId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderCatalogs_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderCatalogs_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderCatalogs_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderCatalogs_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalLibrarys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecialityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    LibraryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalLibrarys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalLibrarys_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_AggregateTypeId",
                table: "MixDesigns",
                column: "AggregateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_CementTypeId",
                table: "MixDesigns",
                column: "CementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_ConcreteUseId",
                table: "MixDesigns",
                column: "ConcreteUseId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_TechnicalVersionId",
                table: "MixDesigns",
                column: "TechnicalVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_ProviderId",
                table: "ProviderCatalogs",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_SpecialityId",
                table: "ProviderCatalogs",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_SupplyFamilyId",
                table: "ProviderCatalogs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_SupplyGroupId",
                table: "ProviderCatalogs",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalLibrarys_SpecialityId",
                table: "TechnicalLibrarys",
                column: "SpecialityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixDesigns");

            migrationBuilder.DropTable(
                name: "ProviderCatalogs");

            migrationBuilder.DropTable(
                name: "TechnicalLibrarys");
        }
    }
}
