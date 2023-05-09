using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreatePreRequestTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PreRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    CorrelativeCode = table.Column<int>(nullable: false),
                    CorrelativePrefix = table.Column<string>(nullable: true),
                    OrderStatus = table.Column<int>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: true),
                    ProjectFormulaId = table.Column<Guid>(nullable: true),
                    RequestType = table.Column<int>(nullable: false),
                    IssueDate = table.Column<DateTime>(nullable: true),
                    DeliveryDate = table.Column<DateTime>(nullable: true),
                    RequestDeliveryPlaceId = table.Column<Guid>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    AttentionStatus = table.Column<int>(nullable: false),
                    IssuedUserId = table.Column<string>(nullable: true),
                    QualityCertificate = table.Column<bool>(nullable: false),
                    Blueprint = table.Column<bool>(nullable: false),
                    TechnicalInformation = table.Column<bool>(nullable: false),
                    CalibrationCertificate = table.Column<bool>(nullable: false),
                    Catalog = table.Column<bool>(nullable: false),
                    Other = table.Column<bool>(nullable: false),
                    OtherDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRequests_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequests_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                        column: x => x.RequestDeliveryPlaceId,
                        principalTable: "RequestDeliveryPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequests_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestUsers_PreRequestId",
                table: "RequestUsers",
                column: "PreRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_BudgetTitleId",
                table: "PreRequests",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_ProjectFormulaId",
                table: "PreRequests",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_ProjectId",
                table: "PreRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_RequestDeliveryPlaceId",
                table: "PreRequests",
                column: "RequestDeliveryPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequests_SupplyFamilyId",
                table: "PreRequests",
                column: "SupplyFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId",
                principalTable: "PreRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUsers_PreRequests_PreRequestId",
                table: "RequestUsers",
                column: "PreRequestId",
                principalTable: "PreRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUsers_PreRequests_PreRequestId",
                table: "RequestUsers");

            migrationBuilder.DropTable(
                name: "PreRequests");

            migrationBuilder.DropIndex(
                name: "IX_RequestUsers_PreRequestId",
                table: "RequestUsers");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "PreRequestId",
                table: "RequestUsers");

            migrationBuilder.DropColumn(
                name: "PreRequestId",
                table: "RequestItems");
        }
    }
}
