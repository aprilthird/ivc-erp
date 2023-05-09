using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_PreRequestItems_PreRequestUsers_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_PreRequests_PreRequestId",
                table: "RequestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUsers_PreRequests_PreRequestId",
                table: "RequestUsers");

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

            migrationBuilder.CreateTable(
                name: "PreRequestItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PreRequestId = table.Column<Guid>(nullable: false),
                    GoalBudgetInputId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    Measure = table.Column<double>(nullable: false),
                    UsedFor = table.Column<string>(nullable: true),
                    Observations = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRequestItems_GoalBudgetInputs_GoalBudgetInputId",
                        column: x => x.GoalBudgetInputId,
                        principalTable: "GoalBudgetInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequestItems_PreRequests_PreRequestId",
                        column: x => x.PreRequestId,
                        principalTable: "PreRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRequestItems_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreRequestUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PreRequestId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(maxLength: 256, nullable: false),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequestUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRequestUsers_PreRequests_PreRequestId",
                        column: x => x.PreRequestId,
                        principalTable: "PreRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestItems_GoalBudgetInputId",
                table: "PreRequestItems",
                column: "GoalBudgetInputId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestItems_PreRequestId",
                table: "PreRequestItems",
                column: "PreRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestItems_WorkFrontId",
                table: "PreRequestItems",
                column: "WorkFrontId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestUsers_PreRequestId",
                table: "PreRequestUsers",
                column: "PreRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRequestItems");

            migrationBuilder.DropTable(
                name: "PreRequestUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreRequestId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestUsers_PreRequestId",
                table: "RequestUsers",
                column: "PreRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_PreRequestId",
                table: "RequestItems",
                column: "PreRequestId");

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
    }
}
