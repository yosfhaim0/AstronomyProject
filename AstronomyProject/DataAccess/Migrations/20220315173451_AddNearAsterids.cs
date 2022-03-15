using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddNearAsterids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NearAsteroids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AbsoluteMagnitudeH = table.Column<double>(type: "float", nullable: false),
                    IsPotentiallyHazardousAsteroid = table.Column<bool>(type: "bit", nullable: false),
                    IsSentryObject = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedDiameterMax = table.Column<double>(type: "float", nullable: false),
                    EstimatedDiameterMin = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NearAsteroids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloseApproachs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloseApproachDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RelativeVelocity = table.Column<double>(type: "float", nullable: false),
                    MissDistance = table.Column<double>(type: "float", nullable: false),
                    OrbitingBody = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NearAsteroidId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloseApproachs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloseApproachs_NearAsteroids_NearAsteroidId",
                        column: x => x.NearAsteroidId,
                        principalTable: "NearAsteroids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloseApproachs_NearAsteroidId",
                table: "CloseApproachs",
                column: "NearAsteroidId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloseApproachs");

            migrationBuilder.DropTable(
                name: "NearAsteroids");
        }
    }
}
