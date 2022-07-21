using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class GroupFilesFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupFiles_Files_FileId",
                table: "GroupFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupFiles_Files_FileId",
                table: "GroupFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupFiles_Files_FileId",
                table: "GroupFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupFiles_Files_FileId",
                table: "GroupFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
