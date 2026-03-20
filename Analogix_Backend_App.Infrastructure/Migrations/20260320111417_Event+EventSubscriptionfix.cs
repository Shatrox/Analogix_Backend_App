using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analogix_Backend_App.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class EventEventSubscriptionfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
