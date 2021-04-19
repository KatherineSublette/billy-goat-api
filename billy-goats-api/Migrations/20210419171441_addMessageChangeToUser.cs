using Microsoft.EntityFrameworkCore.Migrations;

namespace BillyGoats.Api.Migrations
{
    public partial class addMessageChangeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_guests_guest_id",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "fk_messages_guides_guide_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "guide_id",
                table: "messages",
                newName: "sender_id");

            migrationBuilder.RenameColumn(
                name: "guest_id",
                table: "messages",
                newName: "recipient_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_guide_id",
                table: "messages",
                newName: "IX_messages_sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_guest_id",
                table: "messages",
                newName: "IX_messages_recipient_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_users_recipient_id",
                table: "messages",
                column: "recipient_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_users_sender_id",
                table: "messages",
                column: "sender_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_users_recipient_id",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "fk_messages_users_sender_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "sender_id",
                table: "messages",
                newName: "guide_id");

            migrationBuilder.RenameColumn(
                name: "recipient_id",
                table: "messages",
                newName: "guest_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                newName: "IX_messages_guide_id");

            migrationBuilder.RenameIndex(
                name: "IX_messages_recipient_id",
                table: "messages",
                newName: "IX_messages_guest_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_guests_guest_id",
                table: "messages",
                column: "guest_id",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_guides_guide_id",
                table: "messages",
                column: "guide_id",
                principalTable: "guides",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
