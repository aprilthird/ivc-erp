using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BiddingV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Professionals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CapacitationUrl",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CertiAdult",
                table: "Professionals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CertiAdultUrl",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CipDate",
                table: "Professionals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CipUrl",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DniUrl",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTitle",
                table: "Professionals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TitleUrl",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidationSunedu",
                table: "Professionals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BiddingBusinesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessId = table.Column<Guid>(nullable: true),
                    BiddingBusinessName = table.Column<string>(nullable: true),
                    BiddingBusinessRuc = table.Column<string>(nullable: true),
                    BidddingBusinessCreationDate = table.Column<DateTime>(nullable: false),
                    LegalRepresentant = table.Column<string>(nullable: true),
                    Ruc = table.Column<string>(nullable: true),
                    RucUrl = table.Column<string>(nullable: true),
                    TestimonyUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingBusinesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiddingBusinesses_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BiddingWorkTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingWorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BiddingWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    BiddingWorkTypeId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DifDate = table.Column<int>(nullable: false),
                    ReceivedDate = table.Column<DateTime>(nullable: false),
                    LiquidationDate = table.Column<DateTime>(nullable: false),
                    BiddingBusinessId = table.Column<Guid>(nullable: false),
                    ContractUrl = table.Column<string>(nullable: true),
                    ReceivedActUrl = table.Column<string>(nullable: true),
                    LiquidationUrl = table.Column<string>(nullable: true),
                    InVoiceUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiddingWorks_BiddingWorkTypes_BiddingWorkTypeId",
                        column: x => x.BiddingWorkTypeId,
                        principalTable: "BiddingWorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalExperienceFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfessionalId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    BiddingBusinessId = table.Column<Guid>(nullable: false),
                    BiddingWorkId = table.Column<Guid>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DifDate = table.Column<DateTime>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalExperienceFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalExperienceFoldings_BiddingBusinesses_BiddingBusinessId",
                        column: x => x.BiddingBusinessId,
                        principalTable: "BiddingBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalExperienceFoldings_BiddingWorks_BiddingWorkId",
                        column: x => x.BiddingWorkId,
                        principalTable: "BiddingWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalExperienceFoldings_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalExperienceFoldings_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiddingBusinesses_BusinessId",
                table: "BiddingBusinesses",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorks_BiddingWorkTypeId",
                table: "BiddingWorks",
                column: "BiddingWorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_BiddingBusinessId",
                table: "ProfessionalExperienceFoldings",
                column: "BiddingBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_BiddingWorkId",
                table: "ProfessionalExperienceFoldings",
                column: "BiddingWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_PositionId",
                table: "ProfessionalExperienceFoldings",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_ProfessionalId",
                table: "ProfessionalExperienceFoldings",
                column: "ProfessionalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionalExperienceFoldings");

            migrationBuilder.DropTable(
                name: "BiddingBusinesses");

            migrationBuilder.DropTable(
                name: "BiddingWorks");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "BiddingWorkTypes");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CapacitationUrl",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CertiAdult",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CertiAdultUrl",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CipDate",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CipUrl",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "DniUrl",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "StartTitle",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "TitleUrl",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "ValidationSunedu",
                table: "Professionals");
        }
    }
}
