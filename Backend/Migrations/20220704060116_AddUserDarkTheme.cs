using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class AddUserDarkTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DarkTheme",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkTheme",
                table: "Users");
        }
    }
}
