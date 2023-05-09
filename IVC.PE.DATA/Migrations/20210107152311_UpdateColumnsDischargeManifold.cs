using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnsDischargeManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Book",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "Seat",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "TopographicalEquipment",
                table: "DischargeManifolds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenZTest",
                table: "DischargeManifolds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MirrorTest",
                table: "DischargeManifolds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedZTest",
                table: "DischargeManifolds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BallTest",
                table: "DischargeManifolds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "BookPZF",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookPZO",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtocolNumber",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatPZC",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatPZF",
                table: "DischargeManifolds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateRenewalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificateRenewals_EquipmentCertificateRenewalId",
                table: "DischargeManifolds",
                column: "EquipmentCertificateRenewalId",
                principalTable: "EquipmentCertificateRenewals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DischargeManifolds_EquipmentCertificateRenewals_EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.DropIndex(
                name: "IX_DischargeManifolds_EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "BookPZF",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "BookPZO",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "EquipmentCertificateRenewalId",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "ProtocolNumber",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "SeatPZC",
                table: "DischargeManifolds");

            migrationBuilder.DropColumn(
                name: "SeatPZF",
                table: "DischargeManifolds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenZTest",
                table: "DischargeManifolds",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MirrorTest",
                table: "DischargeManifolds",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedZTest",
                table: "DischargeManifolds",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BallTest",
                table: "DischargeManifolds",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book",
                table: "DischargeManifolds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seat",
                table: "DischargeManifolds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopographicalEquipment",
                table: "DischargeManifolds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
