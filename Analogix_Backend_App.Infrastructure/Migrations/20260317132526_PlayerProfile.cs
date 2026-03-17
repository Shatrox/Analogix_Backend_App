using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analogix_Backend_App.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class PlayerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Biography = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    FavoriteGames = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MasteryLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_UserId",
                table: "PlayerProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerProfiles");
        }
    }
}
