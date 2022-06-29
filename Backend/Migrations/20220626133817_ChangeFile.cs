using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class ChangeFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Files_FileId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Groups",
                newName: "ProfileImageId");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Groups",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                newName: "IX_Groups_ProfileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_FileId",
                table: "Groups",
                newName: "IX_Groups_AdminId");

            migrationBuilder.AddColumn<int>(
                name: "FilePolicy",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInfoMessage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_GroupId",
                table: "GroupMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_SenderId",
                table: "GroupMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_ProfileImageId",
                table: "Groups",
                column: "ProfileImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_AdminId",
                table: "Groups",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Files_ProfileImageId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_AdminId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "FilePolicy",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "Groups",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Groups",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ProfileImageId",
                table: "Groups",
                newName: "IX_Groups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_AdminId",
                table: "Groups",
                newName: "IX_Groups_FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_FileId",
                table: "Groups",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
