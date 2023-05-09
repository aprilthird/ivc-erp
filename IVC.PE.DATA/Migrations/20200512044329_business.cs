using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class business : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    BusinessName = table.Column<string>(nullable: true),
                    Tradename = table.Column<string>(nullable: true),
                    RUC = table.Column<string>(nullable: true),
                    LegalAgent = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    CollectionAreaContactName = table.Column<string>(nullable: true),
                    CollectionAreaEmail = table.Column<string>(nullable: true),
                    CollectionAreaPhoneNumber = table.Column<string>(nullable: true),
                    BankId = table.Column<Guid>(nullable: true),
                    BankAccountType = table.Column<int>(nullable: false),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankAccountCCI = table.Column<string>(nullable: true),
                    ForeignBankId = table.Column<Guid>(nullable: true),
                    ForeignBankAccountType = table.Column<int>(nullable: false),
                    ForeignBankAccountCurrency = table.Column<int>(nullable: false),
                    ForeignBankAccountNumber = table.Column<string>(nullable: true),
                    ForeignBankAccountCCI = table.Column<string>(nullable: true),
                    TaxBankId = table.Column<Guid>(nullable: true),
                    TaxBankAccountNumber = table.Column<string>(nullable: true),
                    PropertyServiceType = table.Column<int>(nullable: false),
                    PropertyServiceCode = table.Column<string>(nullable: true),
                    SupplyFamilyId = table.Column<Guid>(nullable: true),
                    SupplyGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Businesses_Banks_ForeignBankId",
                        column: x => x.ForeignBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Businesses_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Businesses_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Businesses_Banks_TaxBankId",
                        column: x => x.TaxBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    ProviderId = table.Column<Guid>(nullable: false),
                    BusinessId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessFiles_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessFiles_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_BankId",
                table: "Businesses",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_ForeignBankId",
                table: "Businesses",
                column: "ForeignBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_SupplyFamilyId",
                table: "Businesses",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_SupplyGroupId",
                table: "Businesses",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_TaxBankId",
                table: "Businesses",
                column: "TaxBankId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessFiles_BusinessId",
                table: "BusinessFiles",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessFiles_ProviderId",
                table: "BusinessFiles",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessFiles");

            migrationBuilder.DropTable(
                name: "Businesses");
        }
    }
}
