using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_PreRequestAuthorizationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestUsers_Requests_RequestId",
                table: "RequestUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUsers_AspNetUsers_UserId",
                table: "RequestUsers");

            migrationBuilder.DropTable(
                name: "PreRequestUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestUsers",
                table: "RequestUsers");

            migrationBuilder.RenameTable(
                name: "RequestUsers",
                newName: "RequestUser");

            migrationBuilder.RenameIndex(
                name: "IX_RequestUsers_RequestId",
                table: "RequestUser",
                newName: "IX_RequestUser_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestUser",
                table: "RequestUser",
                columns: new[] { "UserId", "RequestId" });

            migrationBuilder.CreateTable(
                name: "PreRequestAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PreRequestId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRequestAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRequestAuthorizations_PreRequests_PreRequestId",
                        column: x => x.PreRequestId,
                        principalTable: "PreRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestAuthorizations_PreRequestId",
                table: "PreRequestAuthorizations",
                column: "PreRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUser_Requests_RequestId",
                table: "RequestUser",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUser_AspNetUsers_UserId",
                table: "RequestUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestUser_Requests_RequestId",
                table: "RequestUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUser_AspNetUsers_UserId",
                table: "RequestUser");

            migrationBuilder.DropTable(
                name: "PreRequestAuthorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestUser",
                table: "RequestUser");

            migrationBuilder.RenameTable(
                name: "RequestUser",
                newName: "RequestUsers");

            migrationBuilder.RenameIndex(
                name: "IX_RequestUser_RequestId",
                table: "RequestUsers",
                newName: "IX_RequestUsers_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestUsers",
                table: "RequestUsers",
                columns: new[] { "UserId", "RequestId" });

            migrationBuilder.CreateTable(
                name: "PreRequestUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                name: "IX_PreRequestUsers_PreRequestId",
                table: "PreRequestUsers",
                column: "PreRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUsers_Requests_RequestId",
                table: "RequestUsers",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUsers_AspNetUsers_UserId",
                table: "RequestUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
