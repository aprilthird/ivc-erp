using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CostCenterAndWorkFrontHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Employees_EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFronts_AspNetUsers_UserId",
                table: "WorkFronts");

            migrationBuilder.DropIndex(
                name: "IX_WorkFronts_UserId",
                table: "WorkFronts");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkFronts");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WorkFronts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCenter",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkFrontHeads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFrontHeads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFrontHeads_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFrontHeads_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFronts_ProjectId",
                table: "WorkFronts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkFrontHeadId",
                table: "SewerGroups",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontHeads_ProjectId",
                table: "WorkFrontHeads",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontHeads_UserId",
                table: "WorkFrontHeads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroups",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFronts_Projects_ProjectId",
                table: "WorkFronts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_WorkFrontHeads_WorkFrontHeadId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFronts_Projects_ProjectId",
                table: "WorkFronts");

            migrationBuilder.DropTable(
                name: "WorkFrontHeads");

            migrationBuilder.DropIndex(
                name: "IX_WorkFronts_ProjectId",
                table: "WorkFronts");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_WorkFrontHeadId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkFronts");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkFronts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFronts_UserId",
                table: "WorkFronts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_EmployeeId",
                table: "SewerGroups",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Employees_EmployeeId",
                table: "SewerGroups",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFronts_AspNetUsers_UserId",
                table: "WorkFronts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
