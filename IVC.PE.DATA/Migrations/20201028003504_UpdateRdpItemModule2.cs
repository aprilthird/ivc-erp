using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRdpItemModule2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RdpItemFootages_Projects_ProjectId",
                table: "RdpItemFootages");

            migrationBuilder.DropTable(
                name: "RdpItemTotals");

            migrationBuilder.DropIndex(
                name: "IX_RdpItemFootages_ProjectId",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "IsDetail",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "Accumulated",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "Contractual",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "RdpItemFootages");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemContractualAmmount",
                table: "RdpItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "RdpItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemGroup",
                table: "RdpItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ItemPhaseCode",
                table: "RdpItems",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemStakeOutAmmount",
                table: "RdpItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "RdpItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "AccumulatedAmmount",
                table: "RdpItemFootages",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PartialAmmount",
                table: "RdpItemFootages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RdpItems_ProjectPhaseId",
                table: "RdpItems",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RdpItems_ProjectPhases_ProjectPhaseId",
                table: "RdpItems",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RdpItems_ProjectPhases_ProjectPhaseId",
                table: "RdpItems");

            migrationBuilder.DropIndex(
                name: "IX_RdpItems_ProjectPhaseId",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ItemContractualAmmount",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ItemGroup",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ItemPhaseCode",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ItemStakeOutAmmount",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "RdpItems");

            migrationBuilder.DropColumn(
                name: "AccumulatedAmmount",
                table: "RdpItemFootages");

            migrationBuilder.DropColumn(
                name: "PartialAmmount",
                table: "RdpItemFootages");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RdpItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RdpItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDetail",
                table: "RdpItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RdpItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Accumulated",
                table: "RdpItemFootages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Contractual",
                table: "RdpItemFootages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "RdpItemFootages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RdpItemTotals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Accumulated = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Contractual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RdpItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StakeOut = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdpItemTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdpItemTotals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpItemTotals_RdpItems_RdpItemId",
                        column: x => x.RdpItemId,
                        principalTable: "RdpItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_ProjectId",
                table: "RdpItemFootages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemTotals_ProjectId",
                table: "RdpItemTotals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemTotals_RdpItemId",
                table: "RdpItemTotals",
                column: "RdpItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RdpItemFootages_Projects_ProjectId",
                table: "RdpItemFootages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
