using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update2000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Employees_EmployeeId",
                table: "Letters");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Letters",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Letters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "InterestGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPosition",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryPosition",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NewAccount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoEmail",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PensionFundAdministratorId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PensionFundUniqueIdentificationCode",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PensionFundAdministrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PensionFundAdministrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInterestGroups",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    InterestGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterestGroups", x => new { x.UserId, x.InterestGroupId });
                    table.ForeignKey(
                        name: "FK_UserInterestGroups_InterestGroups_InterestGroupId",
                        column: x => x.InterestGroupId,
                        principalTable: "InterestGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInterestGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestGroups_ProjectId",
                table: "InterestGroups",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PensionFundAdministratorId",
                table: "AspNetUsers",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterestGroups_InterestGroupId",
                table: "UserInterestGroups",
                column: "InterestGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "AspNetUsers",
                column: "PensionFundAdministratorId",
                principalTable: "PensionFundAdministrators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestGroups_Projects_ProjectId",
                table: "InterestGroups",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_AspNetUsers_EmployeeId",
                table: "Letters",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestGroups_Projects_ProjectId",
                table: "InterestGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_AspNetUsers_EmployeeId",
                table: "Letters");

            migrationBuilder.DropTable(
                name: "PensionFundAdministrators");

            migrationBuilder.DropTable(
                name: "UserInterestGroups");

            migrationBuilder.DropIndex(
                name: "IX_InterestGroups_ProjectId",
                table: "InterestGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PensionFundAdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "InterestGroups");

            migrationBuilder.DropColumn(
                name: "CurrentPosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntryPosition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NewAccount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NoEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionFundAdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionFundUniqueIdentificationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "AspNetUsers");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "EmployeeId",
            //    table: "Letters",
            //    type: "uniqueidentifier",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Letters_Employees_EmployeeId",
            //    table: "Letters",
            //    column: "EmployeeId",
            //    principalTable: "Employees",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
