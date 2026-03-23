using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analogix_Backend_App.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RatingAndFaqFinished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventFaqs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorUserId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AskedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AnsweredUserId = table.Column<long>(type: "bigint", nullable: true),
                    AnsweredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventFaqs_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventFaqs_Users_AnsweredUserId",
                        column: x => x.AnsweredUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventFaqs_Users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFaqs_AnsweredUserId",
                table: "EventFaqs",
                column: "AnsweredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventFaqs_AuthorUserId",
                table: "EventFaqs",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventFaqs_EventId",
                table: "EventFaqs",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFaqs");
        }
    }
}
