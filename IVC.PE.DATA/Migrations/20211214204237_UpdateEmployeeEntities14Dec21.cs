using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateEmployeeEntities14Dec21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkPositions_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkPositions_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EntryPositionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CurrentPositionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EntryPositionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PensionFundUniqueIdentificationCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountCci",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HaveHouseholdAllowance",
                table: "Employees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeeWorkPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    PensionFundAdministratorId = table.Column<Guid>(nullable: true),
                    PensionFundUniqueIdentificationCode = table.Column<string>(nullable: true),
                    WorkArea = table.Column<int>(nullable: false),
                    WorkerPositionId = table.Column<Guid>(nullable: true),
                    HasSctr = table.Column<bool>(nullable: false),
                    SctrHealthType = table.Column<int>(nullable: false),
                    SctrPensionType = table.Column<int>(nullable: false),
                    JudicialRetentionFixedAmmount = table.Column<decimal>(nullable: false),
                    JudicialRetentionPercentRate = table.Column<decimal>(nullable: false),
                    LaborRegimen = table.Column<int>(nullable: false),
                    HasEPS = table.Column<bool>(nullable: false),
                    HasEsSaludPlusVida = table.Column<bool>(nullable: false),
                    CeaseDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPeriods_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPeriods_PensionFundAdministrators_PensionFundAdministratorId",
                        column: x => x.PensionFundAdministratorId,
                        principalTable: "PensionFundAdministrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPeriods_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPeriods_WorkPositions_WorkerPositionId",
                        column: x => x.WorkerPositionId,
                        principalTable: "WorkPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BankId",
                table: "Employees",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPeriods_EmployeeId",
                table: "EmployeeWorkPeriods",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPeriods_PensionFundAdministratorId",
                table: "EmployeeWorkPeriods",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPeriods_ProjectId",
                table: "EmployeeWorkPeriods",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPeriods_WorkerPositionId",
                table: "EmployeeWorkPeriods",
                column: "WorkerPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Banks_BankId",
                table: "Employees",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Banks_BankId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeWorkPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Employees_BankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BankAccountCci",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "HaveHouseholdAllowance",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentPositionId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntryPositionId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PensionFundUniqueIdentificationCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CurrentPositionId",
                table: "Employees",
                column: "CurrentPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EntryPositionId",
                table: "Employees",
                column: "EntryPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkPositions_CurrentPositionId",
                table: "Employees",
                column: "CurrentPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkPositions_EntryPositionId",
                table: "Employees",
                column: "EntryPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
