using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class change2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_Banks_BankId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_BondGuarantors_BondGuarantorId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_BondRenovations_BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_BondTypes_BondTypeId",
                table: "BondAdds");

            migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_BudgetTitles_BudgetTitleId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BankId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BondGuarantorId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BondTypeId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_BudgetTitleId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BondGuarantorId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BondNumber",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BondRenovationId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BondTypeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "PenAmmount",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "UsdAmmount",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "daysLimitTerm",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "guaranteeDesc",
                table: "BondAdds");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "BondAdds",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDirectCost",
                table: "BondAdds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BondAdds",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BondAdds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_ProjectId",
                table: "BondAdds",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_Projects_ProjectId",
                table: "BondAdds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.DropForeignKey(
                name: "FK_BondAdds_Projects_ProjectId",
                table: "BondAdds");

            migrationBuilder.DropIndex(
                name: "IX_BondAdds_ProjectId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "IsDirectCost",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BondAdds");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BondGuarantorId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "BondNumber",
                table: "BondAdds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BondRenovationId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BondTypeId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
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

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondAdds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        
            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_Banks_BankId",
                table: "BondAdds",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_BondGuarantors_BondGuarantorId",
                table: "BondAdds",
                column: "BondGuarantorId",
                principalTable: "BondGuarantors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_BondRenovations_BondRenovationId",
                table: "BondAdds",
                column: "BondRenovationId",
                principalTable: "BondRenovations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_BondTypes_BondTypeId",
                table: "BondAdds",
                column: "BondTypeId",
                principalTable: "BondTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_BudgetTitles_BudgetTitleId",
                table: "BondAdds",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BondAdds_Employees_EmployeeId",
                table: "BondAdds",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }
    }
}
