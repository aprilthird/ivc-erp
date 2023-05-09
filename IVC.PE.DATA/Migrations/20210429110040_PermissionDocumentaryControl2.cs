using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class PermissionDocumentaryControl2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    PrincipalWay = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    AuthorizingEntityId = table.Column<Guid>(nullable: false),
                    Length = table.Column<string>(nullable: true),
                    AuthorizationTypeId = table.Column<Guid>(nullable: false),
                    NumberOfPermissions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_AuthorizationTypes_AuthorizationTypeId",
                        column: x => x.AuthorizationTypeId,
                        principalTable: "AuthorizationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_AuthorizingEntities_AuthorizingEntityId",
                        column: x => x.AuthorizingEntityId,
                        principalTable: "AuthorizingEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRenovations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false),
                    AuthorizationNumber = table.Column<string>(nullable: true),
                    RenovationTypeId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRenovations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionRenovations_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRenovations_RenovationTypes_RenovationTypeId",
                        column: x => x.RenovationTypeId,
                        principalTable: "RenovationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRenovationApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PermissionRenovationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRenovationApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionRenovationApplicationUsers_PermissionRenovations_PermissionRenovationId",
                        column: x => x.PermissionRenovationId,
                        principalTable: "PermissionRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRenovationApplicationUsers_PermissionRenovationId",
                table: "PermissionRenovationApplicationUsers",
                column: "PermissionRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRenovations_PermissionId",
                table: "PermissionRenovations",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRenovations_RenovationTypeId",
                table: "PermissionRenovations",
                column: "RenovationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_AuthorizationTypeId",
                table: "Permissions",
                column: "AuthorizationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_AuthorizingEntityId",
                table: "Permissions",
                column: "AuthorizingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ProjectFormulaId",
                table: "Permissions",
                column: "ProjectFormulaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRenovationApplicationUsers");

            migrationBuilder.DropTable(
                name: "PermissionRenovations");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
