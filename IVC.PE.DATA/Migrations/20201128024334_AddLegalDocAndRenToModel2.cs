using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddLegalDocAndRenToModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalDocumentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessId = table.Column<Guid>(nullable: false),
                    LegalDocumentationTypeId = table.Column<Guid>(nullable: false),
                    NumberOfRenovations = table.Column<int>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    LegalDocumentOrder = table.Column<int>(nullable: false),
                    LegalDocumentationId = table.Column<Guid>(nullable: false),
                    DaysLimitTerm = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Days5 = table.Column<bool>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    IsTheLast = table.Column<bool>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDocumentationRenovations");

            migrationBuilder.DropTable(
                name: "LegalDocumentations");
        }
    }
}
