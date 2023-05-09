using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Banks_BankId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Banks_ForeignBankId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_SupplyFamilies_SupplyFamilyId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_SupplyGroups_SupplyGroupId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Banks_TaxBankId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_BankId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_ForeignBankId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_SupplyFamilyId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_SupplyGroupId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_TaxBankId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BankAccountCCI",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BankAccountType",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountCCI",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountCurrency",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountNumber",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ForeignBankAccountType",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ForeignBankId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PropertyServiceCode",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PropertyServiceType",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "SupplyGroupId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TaxBankAccountNumber",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TaxBankId",
                table: "Businesses");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Businesses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAgent5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RucUrl",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestimonyUrl",
                table: "Businesses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Address5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LegalAgent5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "RucUrl",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TestimonyUrl",
                table: "Businesses");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountCCI",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankAccountType",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForeignBankAccountCCI",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignBankAccountCurrency",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ForeignBankAccountNumber",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignBankAccountType",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ForeignBankId",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyServiceCode",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyServiceType",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyGroupId",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxBankAccountNumber",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaxBankId",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Banks_BankId",
                table: "Businesses",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Banks_ForeignBankId",
                table: "Businesses",
                column: "ForeignBankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_SupplyFamilies_SupplyFamilyId",
                table: "Businesses",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_SupplyGroups_SupplyGroupId",
                table: "Businesses",
                column: "SupplyGroupId",
                principalTable: "SupplyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Banks_TaxBankId",
                table: "Businesses",
                column: "TaxBankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
