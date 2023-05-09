using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LoadLegalDocumentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalDocumentationLoads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessId = table.Column<Guid>(nullable: false),
                    LegalDocumentationTypeId = table.Column<Guid>(nullable: false),
                    LegalDocumentationRenovationId = table.Column<Guid>(nullable: false),
                    DaysLimitTerm = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocumentationLoads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalDocumentationLoads_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalDocumentationLoads_LegalDocumentationRenovations_LegalDocumentationRenovationId",
                        column: x => x.LegalDocumentationRenovationId,
                        principalTable: "LegalDocumentationRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalDocumentationLoads_LegalDocumentationTypes_LegalDocumentationTypeId",
                        column: x => x.LegalDocumentationTypeId,
                        principalTable: "LegalDocumentationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentationLoads_BusinessId",
                table: "LegalDocumentationLoads",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentationLoads_LegalDocumentationRenovationId",
                table: "LegalDocumentationLoads",
                column: "LegalDocumentationRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentationLoads_LegalDocumentationTypeId",
                table: "LegalDocumentationLoads",
                column: "LegalDocumentationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDocumentationLoads");
        }
    }
}
