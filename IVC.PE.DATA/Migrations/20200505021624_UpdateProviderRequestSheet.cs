using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateProviderRequestSheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountCurrency",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProviderFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "RequestItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ForeignBankAccountCCI",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignBankAccountCurrency",
                table: "Providers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ForeignBankAccountNumber",
                table: "Providers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignBankAccountType",
                table: "Providers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ForeignBankId",
                table: "Providers",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RequestItemPhases",
                columns: table => new
                {
                    RequestItemId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItemPhases", x => new { x.RequestItemId, x.ProjectPhaseId });
                    table.ForeignKey(
                        name: "FK_RequestItemPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestItemPhases_RequestItems_RequestItemId",
                        column: x => x.RequestItemId,
                        principalTable: "RequestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestUsers",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestUsers", x => new { x.UserId, x.RequestId });
                    table.ForeignKey(
                        name: "FK_RequestUsers_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_MeasurementUnitId",
                table: "RequestItems",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ForeignBankId",
                table: "Providers",
                column: "ForeignBankId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItemPhases_ProjectPhaseId",
                table: "RequestItemPhases",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestUsers_RequestId",
                table: "RequestUsers",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Banks_ForeignBankId",
                table: "Providers",
                column: "ForeignBankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_MeasurementUnits_MeasurementUnitId",
                table: "RequestItems",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Banks_ForeignBankId",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_MeasurementUnits_MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropTable(
                name: "RequestItemPhases");

            migrationBuilder.DropTable(
                name: "RequestUsers");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ForeignBankId",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountCCI",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountCurrency",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountNumber",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountType",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ForeignBankId",
                table: "Providers");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountCurrency",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProviderFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
