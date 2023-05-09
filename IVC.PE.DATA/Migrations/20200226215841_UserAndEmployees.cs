using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UserAndEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkFronts_WorkFrontHeadId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WorkFrontHeadId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentPosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntryPosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionFundUniqueIdentificationCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "PensionFundAdministratorId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PensionFundUniqueIdentificationCode",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Employees",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "Employees",
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BelongsToMainOffice",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PensionFundAdministratorId",
                table: "Employees",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees",
                column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_UserId",
            //    table: "Employees",
            //    column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_PensionFundAdministrators_PensionFundAdministratorId",
                table: "Employees",
                column: "PensionFundAdministratorId",
                principalTable: "PensionFundAdministrators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Employees_AspNetUsers_UserId",
            //    table: "Employees",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PensionFundAdministrators_PensionFundAdministratorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PensionFundAdministratorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PensionFundAdministratorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PensionFundUniqueIdentificationCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BelongsToMainOffice",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPosition",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryPosition",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PensionFundUniqueIdentificationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkFrontHeadId",
                table: "Employees",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkFronts_WorkFrontHeadId",
                table: "Employees",
                column: "WorkFrontHeadId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
