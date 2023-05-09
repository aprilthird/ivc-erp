using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerBoxManifoldModuleEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SewerBoxes_Code",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "ExcecutionWithDitch",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxEnd",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxStart",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PavementOf2In",
                table: "SewerManifoldLots");

            migrationBuilder.DropColumn(
                name: "PavementOf3In",
                table: "SewerManifoldLots");

            migrationBuilder.DropColumn(
                name: "PavementOf3InMixed",
                table: "SewerManifoldLots");

            migrationBuilder.DropColumn(
                name: "PavementWidth",
                table: "SewerManifoldLots");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SewerBoxes");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerManifolds",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerManifolds",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "ProcessType",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxEndId",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxStartId",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "SewerManifolds",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ProcessType",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SewerBoxType",
                table: "SewerBoxes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_ProjectId",
                table: "SewerManifolds",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerGroupId",
                table: "SewerManifolds",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxes_ProjectId_Code_ProcessType",
                table: "SewerBoxes",
                columns: new[] { "ProjectId", "Code", "ProcessType" });

            migrationBuilder.AddForeignKey(
                name: "FK_SewerBoxes_Projects_ProjectId",
                table: "SewerBoxes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_Projects_ProjectId",
                table: "SewerManifolds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerGroups_SewerGroupId",
                table: "SewerManifolds",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerBoxes_Projects_ProjectId",
                table: "SewerBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_Projects_ProjectId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerGroups_SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_ProjectId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerBoxes_ProjectId_Code_ProcessType",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerBoxes");

            migrationBuilder.DropColumn(
                name: "SewerBoxType",
                table: "SewerBoxes");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExcecutionWithDitch",
                table: "SewerManifolds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxEnd",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxStart",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "PavementOf2In",
                table: "SewerManifoldLots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PavementOf3In",
                table: "SewerManifoldLots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PavementOf3InMixed",
                table: "SewerManifoldLots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PavementWidth",
                table: "SewerManifoldLots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SewerBoxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SewerBoxes_Code",
                table: "SewerBoxes",
                column: "Code",
                unique: true);
        }
    }
}
