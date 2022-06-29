using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TogetherAsk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answered = table.Column<bool>(type: "bit", nullable: false),
                    Answer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TogetherAsk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TogetherConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TogetherConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TogetherSwipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Choice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TogetherSwipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    ProfileImage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoodSubject1 = table.Column<int>(type: "int", nullable: false),
                    GoodSubject2 = table.Column<int>(type: "int", nullable: false),
                    GoodSubject3 = table.Column<int>(type: "int", nullable: false),
                    BadSubject1 = table.Column<int>(type: "int", nullable: false),
                    BadSubject2 = table.Column<int>(type: "int", nullable: false),
                    BadSubject3 = table.Column<int>(type: "int", nullable: false),
                    RefreshExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TogetherAsk");

            migrationBuilder.DropTable(
                name: "TogetherConnections");

            migrationBuilder.DropTable(
                name: "TogetherSwipes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
