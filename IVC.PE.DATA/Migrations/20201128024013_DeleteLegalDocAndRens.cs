using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DeleteLegalDocAndRens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDocumentationRenovations");

            migrationBuilder.DropTable(
                name: "LegalDocumentations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalDocumentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegalDocumentationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfRenovations = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocumentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalDocumentations_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalDocumentations_LegalDocumentationTypes_LegalDocumentationTypeId",
                        column: x => x.LegalDocumentationTypeId,
                        principalTable: "LegalDocumentationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LegalDocumentationRenovations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Days5 = table.Column<bool>(type: "bit", nullable: false),
                    DaysLimitTerm = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTheLast = table.Column<bool>(type: "bit", nullable: false),
                    LegalDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegalDocumentOrder = table.Column<int>(type: "int", nullable: false),
                    LegalDocumentationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocumentationRenovations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalDocumentationRenovations_LegalDocumentations_LegalDocumentationId",
                        column: x => x.LegalDocumentationId,
                        principalTable: "LegalDocumentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentationRenovations_LegalDocumentationId",
                table: "LegalDocumentationRenovations",
                column: "LegalDocumentationId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentations_BusinessId",
                table: "LegalDocumentations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentations_LegalDocumentationTypeId",
                table: "LegalDocumentations",
                column: "LegalDocumentationTypeId");
        }
    }
}
