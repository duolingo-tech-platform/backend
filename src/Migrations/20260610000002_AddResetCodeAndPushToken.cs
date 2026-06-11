using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuolingoTechPlatform.Migrations
{
    public partial class AddResetCodeAndPushToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiry",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PushToken",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ResetCode", table: "Users");
            migrationBuilder.DropColumn(name: "ResetCodeExpiry", table: "Users");
            migrationBuilder.DropColumn(name: "PushToken", table: "Users");
        }
    }
}
