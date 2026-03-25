using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analogix_Backend_App.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class updateDbSchoolReportSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterId = table.Column<long>(type: "bigint", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    ReportedPlayerId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ReportStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewerId = table.Column<long>(type: "bigint", nullable: true),
                    ReviewedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerReports", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_PlayerReports_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerReports_ReportedPlayer",
                        column: x => x.ReportedPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerReports_Reporter",
                        column: x => x.ReporterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerReports_Reviewer",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_Event_Reporter_ReportedPlayer",
                table: "PlayerReports",
                columns: new[] { "EventId", "ReporterId", "ReportedPlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_EventId",
                table: "PlayerReports",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_ReportedPlayerId",
                table: "PlayerReports",
                column: "ReportedPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_ReporterId",
                table: "PlayerReports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_ReportStatus",
                table: "PlayerReports",
                column: "ReportStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReports_ReviewerId",
                table: "PlayerReports",
                column: "ReviewerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerReports");
        }
    }
}
