using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkersUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    PaternalSurname = table.Column<string>(nullable: false),
                    MaternalSurname = table.Column<string>(nullable: false),
                    DocumentType = table.Column<int>(nullable: false),
                    Document = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    PensionFundAdministratorId = table.Column<Guid>(nullable: true),
                    PensionFundUniqueIdentificationCode = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    WorkerPositionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_PensionFundAdministrators_PensionFundAdministratorId",
                        column: x => x.PensionFundAdministratorId,
                        principalTable: "PensionFundAdministrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Workers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Workers_WorkerPositions_WorkerPositionId",
                        column: x => x.WorkerPositionId,
                        principalTable: "WorkerPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_PensionFundAdministratorId",
                table: "Workers",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ProjectId",
                table: "Workers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_WorkerPositionId",
                table: "Workers",
                column: "WorkerPositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
