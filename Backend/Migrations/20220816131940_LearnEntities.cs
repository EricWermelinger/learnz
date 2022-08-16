using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class LearnEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearnSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ended = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnSession_CreateSets_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearnSession_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearnQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearnSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    PossibleAnswers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerByUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnsweredCorrect = table.Column<bool>(type: "bit", nullable: true),
                    MarkedAsHard = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnQuestion_LearnSession_LearnSessionId",
                        column: x => x.LearnSessionId,
                        principalTable: "LearnSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearnQuestion_LearnSessionId",
                table: "LearnQuestion",
                column: "LearnSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnSession_SetId",
                table: "LearnSession",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnSession_UserId",
                table: "LearnSession",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearnQuestion");

            migrationBuilder.DropTable(
                name: "LearnSession");
        }
    }
}
