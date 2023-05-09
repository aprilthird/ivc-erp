using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class GroupsAndPhases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Foremen_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "WorkFronts");

            migrationBuilder.DropColumn(
                name: "ForemanId",
                table: "SewerGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemPhaseId",
                table: "WorkFronts",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "WorkFronts",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerGroups",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Destination",
                table: "SewerGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCollaboratorId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkComponent",
                table: "SewerGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkStructure",
                table: "SewerGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectCollaboratorGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCollaboratorGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPhases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCollaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ProjectCollaboratorGroupId = table.Column<Guid>(nullable: false),
                    PaternalSurname = table.Column<string>(nullable: false),
                    MaternalSurname = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCollaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCollaborators_ProjectCollaboratorGroups_ProjectCollaboratorGroupId",
                        column: x => x.ProjectCollaboratorGroupId,
                        principalTable: "ProjectCollaboratorGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectCollaborators_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_EmployeeId",
                table: "SewerGroups",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ProjectCollaboratorId",
                table: "SewerGroups",
                column: "ProjectCollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCollaborators_ProjectCollaboratorGroupId",
                table: "ProjectCollaborators",
                column: "ProjectCollaboratorGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCollaborators_ProjectId",
                table: "ProjectCollaborators",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhases_ProjectId",
                table: "ProjectPhases",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Employees_EmployeeId",
                table: "SewerGroups",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_ProjectCollaborators_ProjectCollaboratorId",
                table: "SewerGroups",
                column: "ProjectCollaboratorId",
                principalTable: "ProjectCollaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_WorkerId",
                table: "SewerGroups",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Employees_EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_ProjectCollaborators_ProjectCollaboratorId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_WorkerId",
                table: "SewerGroups");

            migrationBuilder.DropTable(
                name: "ProjectCollaborators");

            migrationBuilder.DropTable(
                name: "ProjectPhases");

            migrationBuilder.DropTable(
                name: "ProjectCollaboratorGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ProjectCollaboratorId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ProjectCollaboratorId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "WorkComponent",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "WorkStructure",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "SewerGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemPhaseId",
                table: "WorkFronts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "WorkFronts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "WorkFronts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanId",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Foremen_ForemanId",
                table: "SewerGroups",
                column: "ForemanId",
                principalTable: "Foremen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
