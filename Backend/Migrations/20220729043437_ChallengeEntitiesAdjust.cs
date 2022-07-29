using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class ChallengeEntitiesAdjust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "ChallengeQuestionAnswers",
                newName: "ChallengeQuestionPosedId");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "ChallengeQuestionAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChallengeQuestiosnPosed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeQuestiosnPosed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestiosnPosed_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionAnswers_ChallengeQuestionPosedId",
                table: "ChallengeQuestionAnswers",
                column: "ChallengeQuestionPosedId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestiosnPosed_ChallengeId",
                table: "ChallengeQuestiosnPosed",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeQuestionAnswers_ChallengeQuestiosnPosed_ChallengeQuestionPosedId",
                table: "ChallengeQuestionAnswers",
                column: "ChallengeQuestionPosedId",
                principalTable: "ChallengeQuestiosnPosed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeQuestionAnswers_ChallengeQuestiosnPosed_ChallengeQuestionPosedId",
                table: "ChallengeQuestionAnswers");

            migrationBuilder.DropTable(
                name: "ChallengeQuestiosnPosed");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeQuestionAnswers_ChallengeQuestionPosedId",
                table: "ChallengeQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "ChallengeQuestionAnswers");

            migrationBuilder.RenameColumn(
                name: "ChallengeQuestionPosedId",
                table: "ChallengeQuestionAnswers",
                newName: "QuestionId");
        }
    }
}
