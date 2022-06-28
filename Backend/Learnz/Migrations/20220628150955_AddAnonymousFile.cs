using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class AddAnonymousFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ProfileImageId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "FilesAnonymous",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileNameInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameExternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesAnonymous", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FilesAnonymous_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "FilesAnonymous",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FilesAnonymous_ProfileImageId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "FilesAnonymous");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
