using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class recreatefinance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        /*    migrationBuilder.CreateTable(
                name: "BondGuarantors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondGuarantors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BondRenovations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondRenovations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BondTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BondLoads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    BondGuarantorId = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    BondTypeId = table.Column<Guid>(nullable: false),
                    BankId = table.Column<Guid>(nullable: false),
                    BondNumber = table.Column<string>(nullable: true),
                    BondRenovationId = table.Column<Guid>(nullable: false),
                    PenAmmount = table.Column<double>(nullable: false),
                    UsdAmmount = table.Column<double>(nullable: false),
                    daysLimitTerm = table.Column<int>(nullable: false),
                    guaranteeDesc = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondLoads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BondLoads_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_BondGuarantors_BondGuarantorId",
                        column: x => x.BondGuarantorId,
                        principalTable: "BondGuarantors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_BondRenovations_BondRenovationId",
                        column: x => x.BondRenovationId,
                        principalTable: "BondRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_BondTypes_BondTypeId",
                        column: x => x.BondTypeId,
                        principalTable: "BondTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondLoads_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BankId",
                table: "BondLoads",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BondGuarantorId",
                table: "BondLoads",
                column: "BondGuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BondRenovationId",
                table: "BondLoads",
                column: "BondRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BondTypeId",
                table: "BondLoads",
                column: "BondTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_BudgetTitleId",
                table: "BondLoads",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_EmployeeId",
                table: "BondLoads",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondLoads_ProjectId",
                table: "BondLoads",
                column: "ProjectId");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         /*   migrationBuilder.DropTable(
                name: "BondLoads");

            migrationBuilder.DropTable(
                name: "BondGuarantors");

            migrationBuilder.DropTable(
                name: "BondRenovations");

            migrationBuilder.DropTable(
                name: "BondTypes");*/
        }
    }
}
