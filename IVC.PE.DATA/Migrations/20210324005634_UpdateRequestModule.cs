using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRequestModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_MeasurementUnits_MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_SupplyGroups_SupplyGroupId",
                table: "RequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_SupplyFamilies_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestItemPhases");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_SupplyGroupId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeliveryPlace",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "SupplyGroupId",
                table: "RequestItems");

            migrationBuilder.AddColumn<string>(
                name: "BlueprintsUri",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "Requests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "BudgetUri",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestDeliveryPlaceId",
                table: "Requests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TechincalSpecificationsUri",
                table: "Requests",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetInputId",
                table: "RequestItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RequestSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    TotalOfRequest = table.Column<int>(nullable: false),
                    CodePrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestSummaries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BudgetTitleId",
                table: "Requests",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestDeliveryPlaceId",
                table: "Requests",
                column: "RequestDeliveryPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestSummaries_ProjectId",
                table: "RequestSummaries",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_BudgetTitles_BudgetTitleId",
                table: "Requests",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "Requests",
                column: "RequestDeliveryPlaceId",
                principalTable: "RequestDeliveryPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_BudgetTitles_BudgetTitleId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestDeliveryPlaces_RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestSummaries");

            migrationBuilder.DropIndex(
                name: "IX_Requests_BudgetTitleId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BlueprintsUri",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BudgetUri",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestDeliveryPlaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TechincalSpecificationsUri",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "BudgetType",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryPlace",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetInputId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyGroupId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestItemPhases",
                columns: table => new
                {
                    RequestItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItemPhases", x => new { x.RequestItemId, x.ProjectPhaseId });
                    table.ForeignKey(
                        name: "FK_RequestItemPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestItemPhases_RequestItems_RequestItemId",
                        column: x => x.RequestItemId,
                        principalTable: "RequestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SupplyFamilyId",
                table: "Requests",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_MeasurementUnitId",
                table: "RequestItems",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_SupplyGroupId",
                table: "RequestItems",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItemPhases_ProjectPhaseId",
                table: "RequestItemPhases",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_MeasurementUnits_MeasurementUnitId",
                table: "RequestItems",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_SupplyGroups_SupplyGroupId",
                table: "RequestItems",
                column: "SupplyGroupId",
                principalTable: "SupplyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_SupplyFamilies_SupplyFamilyId",
                table: "Requests",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
