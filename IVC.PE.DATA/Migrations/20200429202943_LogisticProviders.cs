using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LogisticProviders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "SupplyGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "SupplyFamilies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Providers",
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
                    BankId = table.Column<Guid>(nullable: false),
                    BankAccountType = table.Column<int>(nullable: false),
                    BankAccountCurrency = table.Column<int>(nullable: false),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankAccountCCI = table.Column<string>(nullable: true),
                    TaxBankId = table.Column<Guid>(nullable: false),
                    TaxBankAccountNumber = table.Column<string>(nullable: true),
                    PropertyServiceType = table.Column<int>(nullable: false),
                    PropertyServiceCode = table.Column<string>(nullable: true),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Providers_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Providers_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Providers_Banks_TaxBankId",
                        column: x => x.TaxBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    ProviderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderFiles_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderFiles_ProviderId",
                table: "ProviderFiles",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_BankId",
                table: "Providers",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_SupplyFamilyId",
                table: "Providers",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_SupplyGroupId",
                table: "Providers",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_TaxBankId",
                table: "Providers",
                column: "TaxBankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderFiles");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "SupplyGroups");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "SupplyFamilies");
        }
    }
}
