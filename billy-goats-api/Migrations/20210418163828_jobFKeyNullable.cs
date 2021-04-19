using Microsoft.EntityFrameworkCore.Migrations;

namespace BillyGoats.Api.Migrations
{
    public partial class jobFKeyNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jobs_guests_guest_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_guides_guide_id",
                table: "jobs");

            migrationBuilder.AlterColumn<long>(
                name: "guide_id",
                table: "jobs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "guest_id",
                table: "jobs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_guests_guest_id",
                table: "jobs",
                column: "guest_id",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_guides_guide_id",
                table: "jobs",
                column: "guide_id",
                principalTable: "guides",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jobs_guests_guest_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_guides_guide_id",
                table: "jobs");

            migrationBuilder.AlterColumn<long>(
                name: "guide_id",
                table: "jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "guest_id",
                table: "jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_guests_guest_id",
                table: "jobs",
                column: "guest_id",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_guides_guide_id",
                table: "jobs",
                column: "guide_id",
                principalTable: "guides",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
