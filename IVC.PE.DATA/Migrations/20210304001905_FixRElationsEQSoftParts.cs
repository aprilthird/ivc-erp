using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixRElationsEQSoftParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftParts_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "PartDate",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "EndMileage",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropColumn(
                name: "EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropColumn(
                name: "Specific",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropColumn(
                name: "StartMileage",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFoldings",
                table: "EquipmentMachinerySoftParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftPartFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftPartId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    PartDate = table.Column<DateTime>(nullable: false),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    InitMileage = table.Column<string>(nullable: true),
                    EndMileage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftPartFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftPartFoldings_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftPartFoldings_EquipmentMachinerySoftParts_EquipmentMachinerySoftPartId",
                        column: x => x.EquipmentMachinerySoftPartId,
                        principalTable: "EquipmentMachinerySoftParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachinerySoftPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartFoldings_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftPartFoldings",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartFoldings_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartFoldings",
                column: "EquipmentMachinerySoftPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartFoldings_EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachinerySoftPartFoldingId",
                principalTable: "EquipmentMachinerySoftPartFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartFoldings_EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropColumn(
                name: "NumberOfFoldings",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "EquipmentMachinerySoftPartFoldingId",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PartDate",
                table: "EquipmentMachinerySoftParts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "EquipmentMachinerySoftParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EndMileage",
                table: "EquipmentMachinerySoftPartPlusUltra",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Specific",
                table: "EquipmentMachinerySoftPartPlusUltra",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StartMileage",
                table: "EquipmentMachinerySoftPartPlusUltra",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachinerySoftPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftParts_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachinerySoftPartId",
                principalTable: "EquipmentMachinerySoftParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryOperatorId",
                principalTable: "EquipmentMachineryOperators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
