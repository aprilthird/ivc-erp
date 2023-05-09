using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.CreateTable(
                name: "BondAdds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BondGuarantorId = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    BondTypeId = table.Column<Guid>(nullable: false),
                    BankId = table.Column<Guid>(nullable: false),
                    BondNumber = table.Column<string>(nullable: true),
                    BondRenovationId = table.Column<Guid>(nullable: false),
                    PenAmmount = table.Column<double>(nullable: false),
                    UsdAmmount = table.Column<double>(nullable: false),
                    daysLimitTerm = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    guaranteeDesc = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondAdds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BondAdds_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondAdds_BondGuarantors_BondGuarantorId",
                        column: x => x.BondGuarantorId,
                        principalTable: "BondGuarantors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondAdds_BondRenovations_BondRenovationId",
                        column: x => x.BondRenovationId,
                        principalTable: "BondRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondAdds_BondTypes_BondTypeId",
                        column: x => x.BondTypeId,
                        principalTable: "BondTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondAdds_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BondAdds_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BankId",
                table: "BondAdds",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BondGuarantorId",
                table: "BondAdds",
                column: "BondGuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BondRenovationId",
                table: "BondAdds",
                column: "BondRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BondTypeId",
                table: "BondAdds",
                column: "BondTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_BudgetTitleId",
                table: "BondAdds",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds",
                column: "EmployeeId");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        /*    migrationBuilder.DropTable(
                name: "BondAdds");*/
        }
    }
}
