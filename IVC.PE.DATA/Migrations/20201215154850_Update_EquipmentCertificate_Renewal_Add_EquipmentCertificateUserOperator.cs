using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_EquipmentCertificate_Renewal_Add_EquipmentCertificateUserOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificateRenewals_SewerGroups_SewerGroupId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificateRenewals_SewerGroupId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "CalibrationVerificationFrecuency",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "CalibrationVerificationMethod",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificates",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileCalibration",
                table: "EquipmentCertificateRenewals",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentCertificateUserOperators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCertificateUserOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCertificateUserOperators_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals",
                column: "EquipmentCertificateUserOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateUserOperators_ProjectId",
                table: "EquipmentCertificateUserOperators",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificateRenewals_EquipmentCertificateUserOperators_EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals",
                column: "EquipmentCertificateUserOperatorId",
                principalTable: "EquipmentCertificateUserOperators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentCertificateRenewals_EquipmentCertificateUserOperators_EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropTable(
                name: "EquipmentCertificateUserOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentCertificateRenewals_EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateUserOperatorId",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "FileCalibration",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<int>(
                name: "CalibrationVerificationFrecuency",
                table: "EquipmentCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CalibrationVerificationMethod",
                table: "EquipmentCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "EquipmentCertificateRenewals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCertificateRenewals_SewerGroupId",
                table: "EquipmentCertificateRenewals",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentCertificateRenewals_SewerGroups_SewerGroupId",
                table: "EquipmentCertificateRenewals",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
