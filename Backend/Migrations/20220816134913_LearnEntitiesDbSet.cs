using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class LearnEntitiesDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearnQuestion_LearnSession_LearnSessionId",
                table: "LearnQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_LearnSession_CreateSets_SetId",
                table: "LearnSession");

            migrationBuilder.DropForeignKey(
                name: "FK_LearnSession_Users_UserId",
                table: "LearnSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LearnSession",
                table: "LearnSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LearnQuestion",
                table: "LearnQuestion");

            migrationBuilder.RenameTable(
                name: "LearnSession",
                newName: "LearnSessions");

            migrationBuilder.RenameTable(
                name: "LearnQuestion",
                newName: "LearnQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_LearnSession_UserId",
                table: "LearnSessions",
                newName: "IX_LearnSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LearnSession_SetId",
                table: "LearnSessions",
                newName: "IX_LearnSessions_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_LearnQuestion_LearnSessionId",
                table: "LearnQuestions",
                newName: "IX_LearnQuestions_LearnSessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LearnSessions",
                table: "LearnSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LearnQuestions",
                table: "LearnQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearnQuestions_LearnSessions_LearnSessionId",
                table: "LearnQuestions",
                column: "LearnSessionId",
                principalTable: "LearnSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearnSessions_CreateSets_SetId",
                table: "LearnSessions",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearnSessions_Users_UserId",
                table: "LearnSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearnQuestions_LearnSessions_LearnSessionId",
                table: "LearnQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_LearnSessions_CreateSets_SetId",
                table: "LearnSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_LearnSessions_Users_UserId",
                table: "LearnSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LearnSessions",
                table: "LearnSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LearnQuestions",
                table: "LearnQuestions");

            migrationBuilder.RenameTable(
                name: "LearnSessions",
                newName: "LearnSession");

            migrationBuilder.RenameTable(
                name: "LearnQuestions",
                newName: "LearnQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_LearnSessions_UserId",
                table: "LearnSession",
                newName: "IX_LearnSession_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LearnSessions_SetId",
                table: "LearnSession",
                newName: "IX_LearnSession_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_LearnQuestions_LearnSessionId",
                table: "LearnQuestion",
                newName: "IX_LearnQuestion_LearnSessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LearnSession",
                table: "LearnSession",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LearnQuestion",
                table: "LearnQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearnQuestion_LearnSession_LearnSessionId",
                table: "LearnQuestion",
                column: "LearnSessionId",
                principalTable: "LearnSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearnSession_CreateSets_SetId",
                table: "LearnSession",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearnSession_Users_UserId",
                table: "LearnSession",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
