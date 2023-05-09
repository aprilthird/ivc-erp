﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class removeDateTimeColumnsEquipmentCertificateRenewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EquipmentCertificateRenewals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EquipmentCertificateRenewals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EquipmentCertificateRenewals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
