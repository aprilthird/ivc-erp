using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TransportParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTransportId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportParts_EquipmentMachineryTransports_EquipmentMachineryTransportId",
                        column: x => x.EquipmentMachineryTransportId,
                        principalTable: "EquipmentMachineryTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportParts_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                        column: x => x.EquipmentMachineryTypeTransportId,
                        principalTable: "EquipmentMachineryTypeTransports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportParts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportParts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportPartFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportPartId = table.Column<Guid>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    PartDate = table.Column<DateTime>(nullable: false),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    InitMileage = table.Column<string>(nullable: true),
                    EndMileage = table.Column<string>(nullable: true),
                    EquipmentMachineryTypeTransportActivityId = table.Column<Guid>(nullable: true),
                    Specific = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportPartFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportPartFoldings_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportPartFoldings_EquipmentMachineryTransportParts_EquipmentMachineryTransportPartId",
                        column: x => x.EquipmentMachineryTransportPartId,
                        principalTable: "EquipmentMachineryTransportParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportPartFoldings_EquipmentMachineryTypeTransportActivities_EquipmentMachineryTypeTransportActivityId",
                        column: x => x.EquipmentMachineryTypeTransportActivityId,
                        principalTable: "EquipmentMachineryTypeTransportActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTransportPartPlusUltra",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTransportPartFoldingId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTransportActivityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTransportPartPlusUltra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportPartPlusUltra_EquipmentMachineryTransportPartFoldings_EquipmentMachineryTransportPartFoldingId",
                        column: x => x.EquipmentMachineryTransportPartFoldingId,
                        principalTable: "EquipmentMachineryTransportPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTransportPartPlusUltra_EquipmentMachineryTypeTransportActivities_EquipmentMachineryTypeTransportActivityId",
                        column: x => x.EquipmentMachineryTypeTransportActivityId,
                        principalTable: "EquipmentMachineryTypeTransportActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_EquipmentMachineryOperatorId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_EquipmentMachineryTransportPartId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "EquipmentMachineryTransportPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_EquipmentMachineryTypeTransportActivityId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "EquipmentMachineryTypeTransportActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartPlusUltra_EquipmentMachineryTransportPartFoldingId",
                table: "EquipmentMachineryTransportPartPlusUltra",
                column: "EquipmentMachineryTransportPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartPlusUltra_EquipmentMachineryTypeTransportActivityId",
                table: "EquipmentMachineryTransportPartPlusUltra",
                column: "EquipmentMachineryTypeTransportActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportParts_EquipmentMachineryTransportId",
                table: "EquipmentMachineryTransportParts",
                column: "EquipmentMachineryTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportParts_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryTransportParts",
                column: "EquipmentMachineryTypeTransportId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportParts_EquipmentProviderId",
                table: "EquipmentMachineryTransportParts",
                column: "EquipmentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportParts_ProjectId",
                table: "EquipmentMachineryTransportParts",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportPartPlusUltra");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTransportParts");
        }
    }
}
