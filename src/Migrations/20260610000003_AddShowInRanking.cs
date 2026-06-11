using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuolingoTechPlatform.Migrations
{
    public partial class AddShowInRanking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInRanking",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ShowInRanking", table: "Users");
        }
    }
}
