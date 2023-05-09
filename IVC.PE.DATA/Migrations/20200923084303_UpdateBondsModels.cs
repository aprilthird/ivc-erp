using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondsModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_BondRenovations_BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_Employees_EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondFiles_BondAdds_BondAddId",
                table: "BondFiles");

            migrationBuilder.DropIndex(
                name: "IX_BondFiles_BondAddId",
                table: "BondFiles");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BondAddId",
                table: "BondFiles");

            migrationBuilder.DropColumn(
                name: "BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "PenAmmount",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "UsdAmmount",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "currencyType",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "daysLimitTerm",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "guaranteeDesc",
                table: "BondAdds");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BondRenovations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "BondRenovations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "BondRenovations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondRenovations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BondRenovations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "PenAmmount",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UsdAmmount",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "currencyType",
                table: "BondRenovations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "daysLimitTerm",
                table: "BondRenovations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "guaranteeDesc",
                table: "BondRenovations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BondRenovationId",
                table: "BondFiles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NumberOfRenovations",
                table: "BondAdds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RacsReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    Ubication = table.Column<string>(nullable: true),
                    IdentifiesSC = table.Column<bool>(nullable: false),
                    DescriptionIdentifiesSC = table.Column<string>(nullable: true),
                    SCQ01 = table.Column<bool>(nullable: false),
                    SCQ02 = table.Column<bool>(nullable: false),
                    SCQ03 = table.Column<bool>(nullable: false),
                    SCQ04 = table.Column<bool>(nullable: false),
                    SCQ05 = table.Column<bool>(nullable: false),
                    SCQ06 = table.Column<bool>(nullable: false),
                    SCQ07 = table.Column<bool>(nullable: false),
                    SCQ08 = table.Column<bool>(nullable: false),
                    SCQ09 = table.Column<bool>(nullable: false),
                    SCQ10 = table.Column<bool>(nullable: false),
                    SCQ11 = table.Column<bool>(nullable: false),
                    SCQ12 = table.Column<bool>(nullable: false),
                    SCQ13 = table.Column<bool>(nullable: false),
                    SCQ14 = table.Column<bool>(nullable: false),
                    SCQ15 = table.Column<bool>(nullable: false),
                    SCQ16 = table.Column<bool>(nullable: false),
                    SCQ17 = table.Column<bool>(nullable: false),
                    SCQ18 = table.Column<bool>(nullable: false),
                    SCQ19 = table.Column<bool>(nullable: false),
                    SCQ20 = table.Column<bool>(nullable: false),
                    SCQ21 = table.Column<bool>(nullable: false),
                    SCQ22 = table.Column<bool>(nullable: false),
                    SCQ23 = table.Column<bool>(nullable: false),
                    SCQ24 = table.Column<bool>(nullable: false),
                    SCQ25 = table.Column<bool>(nullable: false),
                    SCQ26 = table.Column<bool>(nullable: false),
                    SpecifyConditions = table.Column<string>(nullable: true),
                    IdentifiesSA = table.Column<bool>(nullable: false),
                    DescriptionIdentifiesSA = table.Column<string>(nullable: true),
                    SAQ01 = table.Column<bool>(nullable: false),
                    SAQ02 = table.Column<bool>(nullable: false),
                    SAQ03 = table.Column<bool>(nullable: false),
                    SAQ04 = table.Column<bool>(nullable: false),
                    SAQ05 = table.Column<bool>(nullable: false),
                    SAQ06 = table.Column<bool>(nullable: false),
                    SAQ07 = table.Column<bool>(nullable: false),
                    SAQ08 = table.Column<bool>(nullable: false),
                    SAQ09 = table.Column<bool>(nullable: false),
                    SAQ10 = table.Column<bool>(nullable: false),
                    SAQ11 = table.Column<bool>(nullable: false),
                    SAQ12 = table.Column<bool>(nullable: false),
                    SAQ13 = table.Column<bool>(nullable: false),
                    SAQ14 = table.Column<bool>(nullable: false),
                    SAQ15 = table.Column<bool>(nullable: false),
                    SAQ16 = table.Column<bool>(nullable: false),
                    SAQ17 = table.Column<bool>(nullable: false),
                    SAQ18 = table.Column<bool>(nullable: false),
                    SAQ19 = table.Column<bool>(nullable: false),
                    SpecifyActs = table.Column<string>(nullable: true),
                    ICQ01 = table.Column<bool>(nullable: false),
                    ICQ02 = table.Column<bool>(nullable: false),
                    ICQ03 = table.Column<bool>(nullable: false),
                    ICQ04 = table.Column<bool>(nullable: false),
                    ICQ05 = table.Column<bool>(nullable: false),
                    SpecifyAppliedCorrections = table.Column<string>(nullable: true),
                    SpecifyAnotherAlternative = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    LiftingObservations = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacsReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacsReports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondRenovations_EmployeeId",
                table: "BondRenovations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondFiles_BondRenovationId",
                table: "BondFiles",
                column: "BondRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_RacsReports_ProjectId",
                table: "RacsReports",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondFiles_BondRenovations_BondRenovationId",
                table: "BondFiles",
                column: "BondRenovationId",
                principalTable: "BondRenovations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondRenovations_Employees_EmployeeId",
                table: "BondRenovations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondFiles_BondRenovations_BondRenovationId",
                table: "BondFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BondRenovations_Employees_EmployeeId",
                table: "BondRenovations");

            migrationBuilder.DropTable(
                name: "RacsReports");

            migrationBuilder.DropIndex(
                name: "IX_BondRenovations_EmployeeId",
                table: "BondRenovations");

            migrationBuilder.DropIndex(
                name: "IX_BondFiles_BondRenovationId",
                table: "BondFiles");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "PenAmmount",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "UsdAmmount",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "currencyType",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "daysLimitTerm",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "guaranteeDesc",
                table: "BondRenovations");

            migrationBuilder.DropColumn(
                name: "BondRenovationId",
                table: "BondFiles");

            migrationBuilder.DropColumn(
                name: "NumberOfRenovations",
                table: "BondAdds");

            migrationBuilder.AddColumn<Guid>(
                name: "BondAddId",
                table: "BondFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BondRenovationId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BondAdds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "BondAdds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "BondAdds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BondAdds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "PenAmmount",
                table: "BondAdds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UsdAmmount",
                table: "BondAdds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "currencyType",
                table: "BondAdds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "daysLimitTerm",
                table: "BondAdds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "guaranteeDesc",
                table: "BondAdds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BondFiles_BondAddId",
                table: "BondFiles",
                column: "BondAddId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BondRenovationId",
                table: "BondAdds",
                column: "BondRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_BondRenovations_BondRenovationId",
                table: "BondAdds",
                column: "BondRenovationId",
                principalTable: "BondRenovations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_Employees_EmployeeId",
                table: "BondAdds",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondFiles_BondAdds_BondAddId",
                table: "BondFiles",
                column: "BondAddId",
                principalTable: "BondAdds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
