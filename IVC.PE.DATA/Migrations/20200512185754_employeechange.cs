using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class employeechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         /*   migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "BondAdds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds",
                column: "EmployeeId");
                */
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        
           /* migrationBuilder.DropIndex(
                name: "IX_BondAdds_EmployeeId",
                table: "BondAdds");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BondAdds");*/
        }
    }
}
