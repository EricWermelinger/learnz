using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class FixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TogetherAsk",
                table: "TogetherAsk");

            migrationBuilder.RenameTable(
                name: "TogetherAsk",
                newName: "TogetherAsks");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Users",
                newName: "ProfileImageId");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "TogetherSwipes",
                newName: "SwiperUserId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "TogetherSwipes",
                newName: "AskedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "TogetherAsks",
                newName: "InterestedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "TogetherAsks",
                newName: "AskedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TogetherAsks",
                table: "TogetherAsks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileNameInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameExternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Files_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TogetherMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TogetherMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TogetherMessages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TogetherMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupFiles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Users_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherSwipes_AskedUserId",
                table: "TogetherSwipes",
                column: "AskedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherSwipes_SwiperUserId",
                table: "TogetherSwipes",
                column: "SwiperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherConnections_UserId1",
                table: "TogetherConnections",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherConnections_UserId2",
                table: "TogetherConnections",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherAsks_AskedUserId",
                table: "TogetherAsks",
                column: "AskedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherAsks_InterestedUserId",
                table: "TogetherAsks",
                column: "InterestedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ModifiedById",
                table: "Files",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupFiles_FileId",
                table: "GroupFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupFiles_GroupId",
                table: "GroupFiles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FileId",
                table: "Groups",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherMessages_ReceiverId",
                table: "TogetherMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TogetherMessages_SenderId",
                table: "TogetherMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherAsks_Users_AskedUserId",
                table: "TogetherAsks",
                column: "AskedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherAsks_Users_InterestedUserId",
                table: "TogetherAsks",
                column: "InterestedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherConnections_Users_UserId1",
                table: "TogetherConnections",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherConnections_Users_UserId2",
                table: "TogetherConnections",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherSwipes_Users_AskedUserId",
                table: "TogetherSwipes",
                column: "AskedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TogetherSwipes_Users_SwiperUserId",
                table: "TogetherSwipes",
                column: "SwiperUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TogetherAsks_Users_AskedUserId",
                table: "TogetherAsks");

            migrationBuilder.DropForeignKey(
                name: "FK_TogetherAsks_Users_InterestedUserId",
                table: "TogetherAsks");

            migrationBuilder.DropForeignKey(
                name: "FK_TogetherConnections_Users_UserId1",
                table: "TogetherConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_TogetherConnections_Users_UserId2",
                table: "TogetherConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_TogetherSwipes_Users_AskedUserId",
                table: "TogetherSwipes");

            migrationBuilder.DropForeignKey(
                name: "FK_TogetherSwipes_Users_SwiperUserId",
                table: "TogetherSwipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ProfileImageId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GroupFiles");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "TogetherMessages");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TogetherSwipes_AskedUserId",
                table: "TogetherSwipes");

            migrationBuilder.DropIndex(
                name: "IX_TogetherSwipes_SwiperUserId",
                table: "TogetherSwipes");

            migrationBuilder.DropIndex(
                name: "IX_TogetherConnections_UserId1",
                table: "TogetherConnections");

            migrationBuilder.DropIndex(
                name: "IX_TogetherConnections_UserId2",
                table: "TogetherConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TogetherAsks",
                table: "TogetherAsks");

            migrationBuilder.DropIndex(
                name: "IX_TogetherAsks_AskedUserId",
                table: "TogetherAsks");

            migrationBuilder.DropIndex(
                name: "IX_TogetherAsks_InterestedUserId",
                table: "TogetherAsks");

            migrationBuilder.RenameTable(
                name: "TogetherAsks",
                newName: "TogetherAsk");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "Users",
                newName: "ProfileImage");

            migrationBuilder.RenameColumn(
                name: "SwiperUserId",
                table: "TogetherSwipes",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "AskedUserId",
                table: "TogetherSwipes",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "InterestedUserId",
                table: "TogetherAsk",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "AskedUserId",
                table: "TogetherAsk",
                newName: "UserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TogetherAsk",
                table: "TogetherAsk",
                column: "Id");
        }
    }
}
