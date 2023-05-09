using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerEntityRemovingForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Projects_ProjectId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkPositions_WorkerPositionId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_PensionFundAdministratorId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ProjectId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_WorkerPositionId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Workers",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanWorkerId1",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanWorkerId1",
                table: "SewerGroups",
                column: "ForemanWorkerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId1",
                table: "SewerGroups",
                column: "ForemanWorkerId1",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_PensionFundAdministrators_PensionFundAdministratorId",
                table: "Workers",
                column: "PensionFundAdministratorId",
                principalTable: "PensionFundAdministrators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Projects_ProjectId",
                table: "Workers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_WorkPositions_WorkerPositionId",
                table: "Workers",
                column: "WorkerPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
