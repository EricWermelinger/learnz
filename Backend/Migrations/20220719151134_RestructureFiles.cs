using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class RestructureFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CreatedById",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileNameExternal",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "ActualVersionPath");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                table: "Files",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "FileNameInternal",
                table: "Files",
                newName: "ActualVersionFileNameExternal");

            migrationBuilder.RenameIndex(
                name: "IX_Files_ModifiedById",
                table: "Files",
                newName: "IX_Files_OwnerId");

            migrationBuilder.CreateTable(
                name: "FileVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileNameInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameExternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVersions_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileVersions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileVersions_CreatedById",
                table: "FileVersions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FileVersions_FileId",
                table: "FileVersions",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_OwnerId",
                table: "Files",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_OwnerId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "FileVersions");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Files",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "ActualVersionPath",
                table: "Files",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "ActualVersionFileNameExternal",
                table: "Files",
                newName: "FileNameInternal");

            migrationBuilder.RenameIndex(
                name: "IX_Files_OwnerId",
                table: "Files",
                newName: "IX_Files_ModifiedById");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "FileNameExternal",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
