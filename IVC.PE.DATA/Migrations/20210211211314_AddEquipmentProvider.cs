using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEquipmentProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: true),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: true),
                    SerieNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentProviders_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                        column: x => x.EquipmentMachineryTypeId,
                        principalTable: "EquipmentMachineryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviders_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviders_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviders_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviders_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviders_ProjectId",
                table: "EquipmentProviders",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviders_ProviderId",
                table: "EquipmentProviders",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentProviders");
        }
    }
}
