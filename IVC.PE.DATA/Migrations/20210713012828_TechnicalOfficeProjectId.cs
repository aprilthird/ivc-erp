using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TechnicalOfficeProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TechnicalVersions",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TechnicalSpecs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TechnicalLibrarys",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProviderCatalogs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "MixDesigns",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "DesignTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ConcreteUses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CementTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Blueprints",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Bims",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AggregateTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalVersions_ProjectId",
                table: "TechnicalVersions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecs_ProjectId",
                table: "TechnicalSpecs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalLibrarys_ProjectId",
                table: "TechnicalLibrarys",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_ProjectId",
                table: "Specialities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_ProjectId",
                table: "ProviderCatalogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDesigns_ProjectId",
                table: "MixDesigns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignTypes_ProjectId",
                table: "DesignTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteUses_ProjectId",
                table: "ConcreteUses",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CementTypes_ProjectId",
                table: "CementTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_ProjectId",
                table: "Blueprints",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bims_ProjectId",
                table: "Bims",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregateTypes_ProjectId",
                table: "AggregateTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AggregateTypes_Projects_ProjectId",
                table: "AggregateTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bims_Projects_ProjectId",
                table: "Bims",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_Projects_ProjectId",
                table: "Blueprints",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CementTypes_Projects_ProjectId",
                table: "CementTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConcreteUses_Projects_ProjectId",
                table: "ConcreteUses",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DesignTypes_Projects_ProjectId",
                table: "DesignTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDesigns_Projects_ProjectId",
                table: "MixDesigns",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderCatalogs_Projects_ProjectId",
                table: "ProviderCatalogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_Projects_ProjectId",
                table: "Specialities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalLibrarys_Projects_ProjectId",
                table: "TechnicalLibrarys",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalSpecs_Projects_ProjectId",
                table: "TechnicalSpecs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalVersions_Projects_ProjectId",
                table: "TechnicalVersions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AggregateTypes_Projects_ProjectId",
                table: "AggregateTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Bims_Projects_ProjectId",
                table: "Bims");

            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_Projects_ProjectId",
                table: "Blueprints");

            migrationBuilder.DropForeignKey(
                name: "FK_CementTypes_Projects_ProjectId",
                table: "CementTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConcreteUses_Projects_ProjectId",
                table: "ConcreteUses");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignTypes_Projects_ProjectId",
                table: "DesignTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDesigns_Projects_ProjectId",
                table: "MixDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderCatalogs_Projects_ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_Projects_ProjectId",
                table: "Specialities");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalLibrarys_Projects_ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalSpecs_Projects_ProjectId",
                table: "TechnicalSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalVersions_Projects_ProjectId",
                table: "TechnicalVersions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalVersions_ProjectId",
                table: "TechnicalVersions");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalSpecs_ProjectId",
                table: "TechnicalSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalLibrarys_ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_ProjectId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_ProviderCatalogs_ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropIndex(
                name: "IX_MixDesigns_ProjectId",
                table: "MixDesigns");

            migrationBuilder.DropIndex(
                name: "IX_DesignTypes_ProjectId",
                table: "DesignTypes");

            migrationBuilder.DropIndex(
                name: "IX_ConcreteUses_ProjectId",
                table: "ConcreteUses");

            migrationBuilder.DropIndex(
                name: "IX_CementTypes_ProjectId",
                table: "CementTypes");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_ProjectId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Bims_ProjectId",
                table: "Bims");

            migrationBuilder.DropIndex(
                name: "IX_AggregateTypes_ProjectId",
                table: "AggregateTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TechnicalVersions");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TechnicalSpecs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "MixDesigns");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DesignTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ConcreteUses");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CementTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Bims");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AggregateTypes");
        }
    }
}
