using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixdbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftInsuranc~",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser");

            migrationBuilder.RenameTable(
                name: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                newName: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldingId",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                newName: "IX_EquipmentMachinerySoftInsuranceFoldingApplicationUsers_EquipmentMachinerySoftInsuranceFoldingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftInsuranceFoldingApplicationUsers_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftInsuran~",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                column: "EquipmentMachinerySoftInsuranceFoldingId",
                principalTable: "EquipmentMachinerySoftInsuranceFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftInsuranceFoldingApplicationUsers_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftInsuran~",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers");

            migrationBuilder.RenameTable(
                name: "EquipmentMachinerySoftInsuranceFoldingApplicationUsers",
                newName: "EquipmentMachinerySoftInsuranceFoldingApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentMachinerySoftInsuranceFoldingApplicationUsers_EquipmentMachinerySoftInsuranceFoldingId",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                newName: "IX_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftInsuranceFoldingApplicationUser_EquipmentMachinerySoftInsuranceFoldings_EquipmentMachinerySoftInsuranc~",
                table: "EquipmentMachinerySoftInsuranceFoldingApplicationUser",
                column: "EquipmentMachinerySoftInsuranceFoldingId",
                principalTable: "EquipmentMachinerySoftInsuranceFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
