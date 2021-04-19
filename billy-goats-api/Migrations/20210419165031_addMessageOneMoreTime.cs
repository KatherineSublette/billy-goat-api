using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillyGoats.Api.Migrations
{
    public partial class addMessageOneMoreTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guest_id = table.Column<long>(type: "bigint", nullable: true),
                    guide_id = table.Column<long>(type: "bigint", nullable: true),
                    subject = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    read = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_guests_guest_id",
                        column: x => x.guest_id,
                        principalTable: "guests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_messages_guides_guide_id",
                        column: x => x.guide_id,
                        principalTable: "guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_messages_guest_id",
                table: "messages",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_guide_id",
                table: "messages",
                column: "guide_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");
        }
    }
}
