using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillyGoats.Api.Migrations
{
    public partial class AddJobAndResort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "guides",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "guests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "resorts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resorts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resort_id = table.Column<long>(type: "bigint", nullable: false),
                    guest_id = table.Column<long>(type: "bigint", nullable: false),
                    guide_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", maxLength: 75, nullable: false),
                    completed = table.Column<bool>(type: "boolean", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jobs", x => x.id);
                    table.ForeignKey(
                        name: "fk_jobs_guests_guest_id",
                        column: x => x.guest_id,
                        principalTable: "guests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_jobs_guides_guide_id",
                        column: x => x.guide_id,
                        principalTable: "guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_jobs_resorts_resort_id",
                        column: x => x.resort_id,
                        principalTable: "resorts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_guides_user_id",
                table: "guides",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_guests_user_id",
                table: "guests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_guest_id",
                table: "jobs",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_guide_id",
                table: "jobs",
                column: "guide_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_resort_id",
                table: "jobs",
                column: "resort_id");

            migrationBuilder.AddForeignKey(
                name: "fk_guests_users_user_id",
                table: "guests",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_guides_users_user_id",
                table: "guides",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_guests_users_user_id",
                table: "guests");

            migrationBuilder.DropForeignKey(
                name: "fk_guides_users_user_id",
                table: "guides");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "resorts");

            migrationBuilder.DropIndex(
                name: "IX_guides_user_id",
                table: "guides");

            migrationBuilder.DropIndex(
                name: "IX_guests_user_id",
                table: "guests");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "guides",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "guests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
