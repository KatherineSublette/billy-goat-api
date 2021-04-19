using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillyGoats.Api.Migrations
{
    public partial class ChangeDateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "date",
                table: "jobs",
                type: "timestamp with time zone",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldMaxLength: 75);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "jobs",
                type: "timestamp without time zone",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldMaxLength: 75);
        }
    }
}
