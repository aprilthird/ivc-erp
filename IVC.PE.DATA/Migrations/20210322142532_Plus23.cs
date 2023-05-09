using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Plus23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPlusUltra_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPlusUltra_SewerGroups_SewerGroupId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPlusUltra_ProjectPhaseId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPlusUltra_SewerGroupId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "EquipmentMachPlusUltra");

            migrationBuilder.AddColumn<string>(
                name: "Acts",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnHo",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InHo",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatorName",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phs",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepStartDate",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sws",
                table: "EquipmentMachParts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra2",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPlusUltra2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra2_EquipmentMachPartFoldings_EquipmentMachPartFoldingId",
                        column: x => x.EquipmentMachPartFoldingId,
                        principalTable: "EquipmentMachPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra2_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra3",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPlusUltra3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra3_EquipmentMachPartFoldings_EquipmentMachPartFoldingId",
                        column: x => x.EquipmentMachPartFoldingId,
                        principalTable: "EquipmentMachPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra3_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra2_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra2",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra2_SewerGroupId",
                table: "EquipmentMachPlusUltra2",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra3_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra3",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra3_ProjectPhaseId",
                table: "EquipmentMachPlusUltra3",
                column: "ProjectPhaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra2");

            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra3");

            migrationBuilder.DropColumn(
                name: "Acts",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "EnHo",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "InHo",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "OperatorName",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "Phs",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "RepStartDate",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "Sws",
                table: "EquipmentMachParts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachPlusUltra",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "EquipmentMachPlusUltra",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_ProjectPhaseId",
                table: "EquipmentMachPlusUltra",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_SewerGroupId",
                table: "EquipmentMachPlusUltra",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPlusUltra_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPlusUltra",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPlusUltra_SewerGroups_SewerGroupId",
                table: "EquipmentMachPlusUltra",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
