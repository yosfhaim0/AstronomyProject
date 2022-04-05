using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class CreatMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloseApproachs_NearAsteroids_NearAsteroidId",
                table: "CloseApproachs");

            migrationBuilder.AlterColumn<int>(
                name: "NearAsteroidId",
                table: "CloseApproachs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PreviewUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    MediaType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImaggaTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Confidence = table.Column<double>(type: "float", nullable: false),
                    MediaGroupeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImaggaTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImaggaTags_Media_MediaGroupeId",
                        column: x => x.MediaGroupeId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    MediaGroupeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_Media_MediaGroupeId",
                        column: x => x.MediaGroupeId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SearchWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MediaGroupeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchWords_Media_MediaGroupeId",
                        column: x => x.MediaGroupeId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImaggaTags_MediaGroupeId",
                table: "ImaggaTags",
                column: "MediaGroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaGroupeId",
                table: "MediaItems",
                column: "MediaGroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchWords_MediaGroupeId",
                table: "SearchWords",
                column: "MediaGroupeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloseApproachs_NearAsteroids_NearAsteroidId",
                table: "CloseApproachs",
                column: "NearAsteroidId",
                principalTable: "NearAsteroids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloseApproachs_NearAsteroids_NearAsteroidId",
                table: "CloseApproachs");

            migrationBuilder.DropTable(
                name: "ImaggaTags");

            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "SearchWords");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.AlterColumn<int>(
                name: "NearAsteroidId",
                table: "CloseApproachs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CloseApproachs_NearAsteroids_NearAsteroidId",
                table: "CloseApproachs",
                column: "NearAsteroidId",
                principalTable: "NearAsteroids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
