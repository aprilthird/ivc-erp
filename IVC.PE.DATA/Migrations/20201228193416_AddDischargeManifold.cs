using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddDischargeManifold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DischargeManifolds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Section = table.Column<string>(nullable: true),
                    LevelTopI = table.Column<decimal>(nullable: false),
                    LevelBottomI = table.Column<decimal>(nullable: false),
                    LevelTopJ = table.Column<decimal>(nullable: false),
                    LevelBottomJ = table.Column<decimal>(nullable: false),
                    LevelArrivalJ = table.Column<decimal>(nullable: false),
                    LenghtBetweenAxisH = table.Column<decimal>(nullable: false),
                    Producer = table.Column<string>(nullable: true),
                    PipeBatch = table.Column<string>(nullable: true),
                    Leveling = table.Column<DateTime>(nullable: false),
                    OpenZTest = table.Column<DateTime>(nullable: false),
                    ClosedZTest = table.Column<DateTime>(nullable: false),
                    MirrorTest = table.Column<DateTime>(nullable: false),
                    BallTest = table.Column<DateTime>(nullable: false),
                    TopographicalEquipment = table.Column<string>(nullable: true),
                    Book = table.Column<string>(nullable: true),
                    Seat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DischargeManifolds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DischargeManifolds");
        }
    }
}
