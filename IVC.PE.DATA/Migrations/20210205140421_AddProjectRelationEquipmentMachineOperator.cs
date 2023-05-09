using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectRelationEquipmentMachineOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_ProjectId",
                table: "EquipmentMachineryOperators",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_Projects_ProjectId",
                table: "EquipmentMachineryOperators",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_Projects_ProjectId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_ProjectId",
                table: "EquipmentMachineryOperators");
        }
    }
}
