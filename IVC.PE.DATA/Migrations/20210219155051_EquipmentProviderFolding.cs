using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentProviderFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentProviders_EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders");

            migrationBuilder.DropColumn(
                name: "SerieNumber",
                table: "EquipmentProviders");

            migrationBuilder.CreateTable(
                name: "EquipmentProviderFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: true),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: true),
                    SerieNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentProviderFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderFoldings_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                        column: x => x.EquipmentMachineryTypeId,
                        principalTable: "EquipmentMachineryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderFoldings_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderFoldings_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderFoldings_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentMachineryTypeId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentMachineryTypeSoftId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentMachineryTypeTypeId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentMachineryTypeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentProviderId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentProviderFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentProviders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerieNumber",
                table: "EquipmentProviders",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeId",
                principalTable: "EquipmentMachineryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeSoftId",
                principalTable: "EquipmentMachineryTypeSofts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviders_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                table: "EquipmentProviders",
                column: "EquipmentMachineryTypeTypeId",
                principalTable: "EquipmentMachineryTypeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
