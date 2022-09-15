using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class DrawEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestionUsers_TestUsers_TestOfUserId",
                table: "TestQuestionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestUsers_Tests_TestId",
                table: "TestUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestUsers_Users_UserId",
                table: "TestUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestUsers",
                table: "TestUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestionUsers",
                table: "TestQuestionUsers");

            migrationBuilder.RenameTable(
                name: "TestUsers",
                newName: "TestOfUsers");

            migrationBuilder.RenameTable(
                name: "TestQuestionUsers",
                newName: "TestQuestionOfUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TestUsers_UserId",
                table: "TestOfUsers",
                newName: "IX_TestOfUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestUsers_TestId",
                table: "TestOfUsers",
                newName: "IX_TestOfUsers_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestionUsers_TestOfUserId",
                table: "TestQuestionOfUsers",
                newName: "IX_TestQuestionOfUsers_TestOfUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Tests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TestQuestionId",
                table: "TestQuestionOfUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestOfUsers",
                table: "TestOfUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestionOfUsers",
                table: "TestQuestionOfUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DrawCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Changed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrawCollections_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawCollections_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrawGroupCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrawCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrawGroupPolicy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawGroupCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrawGroupCollections_DrawCollections_DrawCollectionId",
                        column: x => x.DrawCollectionId,
                        principalTable: "DrawCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawGroupCollections_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrawPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrawCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Changed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrawPages_DrawCollections_DrawCollectionId",
                        column: x => x.DrawCollectionId,
                        principalTable: "DrawCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawPages_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawPages_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_OwnerId",
                table: "Tests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestionOfUsers_TestQuestionId",
                table: "TestQuestionOfUsers",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawCollections_ChangedById",
                table: "DrawCollections",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DrawCollections_OwnerId",
                table: "DrawCollections",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawGroupCollections_DrawCollectionId",
                table: "DrawGroupCollections",
                column: "DrawCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawGroupCollections_GroupId",
                table: "DrawGroupCollections",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawPages_ChangedById",
                table: "DrawPages",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_DrawPages_DrawCollectionId",
                table: "DrawPages",
                column: "DrawCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawPages_OwnerId",
                table: "DrawPages",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestOfUsers_Tests_TestId",
                table: "TestOfUsers",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestOfUsers_Users_UserId",
                table: "TestOfUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestionOfUsers_TestOfUsers_TestOfUserId",
                table: "TestQuestionOfUsers",
                column: "TestOfUserId",
                principalTable: "TestOfUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestionOfUsers_TestQuestions_TestQuestionId",
                table: "TestQuestionOfUsers",
                column: "TestQuestionId",
                principalTable: "TestQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Users_OwnerId",
                table: "Tests",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestOfUsers_Tests_TestId",
                table: "TestOfUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestOfUsers_Users_UserId",
                table: "TestOfUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestionOfUsers_TestOfUsers_TestOfUserId",
                table: "TestQuestionOfUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestionOfUsers_TestQuestions_TestQuestionId",
                table: "TestQuestionOfUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Users_OwnerId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "DrawGroupCollections");

            migrationBuilder.DropTable(
                name: "DrawPages");

            migrationBuilder.DropTable(
                name: "DrawCollections");

            migrationBuilder.DropIndex(
                name: "IX_Tests_OwnerId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestionOfUsers",
                table: "TestQuestionOfUsers");

            migrationBuilder.DropIndex(
                name: "IX_TestQuestionOfUsers_TestQuestionId",
                table: "TestQuestionOfUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestOfUsers",
                table: "TestOfUsers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestQuestionId",
                table: "TestQuestionOfUsers");

            migrationBuilder.RenameTable(
                name: "TestQuestionOfUsers",
                newName: "TestQuestionUsers");

            migrationBuilder.RenameTable(
                name: "TestOfUsers",
                newName: "TestUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestionOfUsers_TestOfUserId",
                table: "TestQuestionUsers",
                newName: "IX_TestQuestionUsers_TestOfUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestOfUsers_UserId",
                table: "TestUsers",
                newName: "IX_TestUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestOfUsers_TestId",
                table: "TestUsers",
                newName: "IX_TestUsers_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestionUsers",
                table: "TestQuestionUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestUsers",
                table: "TestUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestionUsers_TestUsers_TestOfUserId",
                table: "TestQuestionUsers",
                column: "TestOfUserId",
                principalTable: "TestUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestUsers_Tests_TestId",
                table: "TestUsers",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestUsers_Users_UserId",
                table: "TestUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
