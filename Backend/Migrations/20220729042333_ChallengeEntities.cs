using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class ChallengeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionDistribute_CreateSet_SetId",
                table: "CreateQuestionDistribute");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionDistributeAnswer_CreateQuestionDistribute_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMathematic_CreateSet_SetId",
                table: "CreateQuestionMathematic");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMathematicVariable_CreateQuestionMathematic_QuestionMathematicId",
                table: "CreateQuestionMathematicVariable");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMultipleChoice_CreateSet_SetId",
                table: "CreateQuestionMultipleChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMultipleChoiceAnswer_CreateQuestionMultipleChoice_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionOpenQuestion_CreateSet_SetId",
                table: "CreateQuestionOpenQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionTextField_CreateSet_SetId",
                table: "CreateQuestionTextField");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionTrueFalse_CreateSet_SetId",
                table: "CreateQuestionTrueFalse");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionWord_CreateSet_SetId",
                table: "CreateQuestionWord");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateSet_Users_CreatedById",
                table: "CreateSet");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateSet_Users_ModifiedById",
                table: "CreateSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateSet",
                table: "CreateSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionWord",
                table: "CreateQuestionWord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionTrueFalse",
                table: "CreateQuestionTrueFalse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionTextField",
                table: "CreateQuestionTextField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionOpenQuestion",
                table: "CreateQuestionOpenQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMultipleChoiceAnswer",
                table: "CreateQuestionMultipleChoiceAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMultipleChoice",
                table: "CreateQuestionMultipleChoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMathematicVariable",
                table: "CreateQuestionMathematicVariable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMathematic",
                table: "CreateQuestionMathematic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionDistributeAnswer",
                table: "CreateQuestionDistributeAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionDistribute",
                table: "CreateQuestionDistribute");

            migrationBuilder.RenameTable(
                name: "CreateSet",
                newName: "CreateSets");

            migrationBuilder.RenameTable(
                name: "CreateQuestionWord",
                newName: "CreateQuestionWords");

            migrationBuilder.RenameTable(
                name: "CreateQuestionTrueFalse",
                newName: "CreateQuestionTrueFalses");

            migrationBuilder.RenameTable(
                name: "CreateQuestionTextField",
                newName: "CreateQuestionTextFields");

            migrationBuilder.RenameTable(
                name: "CreateQuestionOpenQuestion",
                newName: "CreateQuestionOpenQuestions");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMultipleChoiceAnswer",
                newName: "CreateQuestionMultipleChoiceAnswers");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMultipleChoice",
                newName: "CreateQuestionMultipleChoices");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMathematicVariable",
                newName: "CreateQuestionMathematicVariables");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMathematic",
                newName: "CreateQuestionMathematics");

            migrationBuilder.RenameTable(
                name: "CreateQuestionDistributeAnswer",
                newName: "CreateQuestionDistributeAnswers");

            migrationBuilder.RenameTable(
                name: "CreateQuestionDistribute",
                newName: "CreateQuestionDistributes");

            migrationBuilder.RenameIndex(
                name: "IX_CreateSet_ModifiedById",
                table: "CreateSets",
                newName: "IX_CreateSets_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_CreateSet_CreatedById",
                table: "CreateSets",
                newName: "IX_CreateSets_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionWord_SetId",
                table: "CreateQuestionWords",
                newName: "IX_CreateQuestionWords_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionTrueFalse_SetId",
                table: "CreateQuestionTrueFalses",
                newName: "IX_CreateQuestionTrueFalses_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionTextField_SetId",
                table: "CreateQuestionTextFields",
                newName: "IX_CreateQuestionTextFields_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionOpenQuestion_SetId",
                table: "CreateQuestionOpenQuestions",
                newName: "IX_CreateQuestionOpenQuestions_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMultipleChoiceAnswer_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswers",
                newName: "IX_CreateQuestionMultipleChoiceAnswers_QuestionMultipleChoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMultipleChoice_SetId",
                table: "CreateQuestionMultipleChoices",
                newName: "IX_CreateQuestionMultipleChoices_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMathematicVariable_QuestionMathematicId",
                table: "CreateQuestionMathematicVariables",
                newName: "IX_CreateQuestionMathematicVariables_QuestionMathematicId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMathematic_SetId",
                table: "CreateQuestionMathematics",
                newName: "IX_CreateQuestionMathematics_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionDistributeAnswer_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswers",
                newName: "IX_CreateQuestionDistributeAnswers_QuestionDistributeId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionDistribute_SetId",
                table: "CreateQuestionDistributes",
                newName: "IX_CreateQuestionDistributes_SetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateSets",
                table: "CreateSets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionWords",
                table: "CreateQuestionWords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionTrueFalses",
                table: "CreateQuestionTrueFalses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionTextFields",
                table: "CreateQuestionTextFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionOpenQuestions",
                table: "CreateQuestionOpenQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMultipleChoiceAnswers",
                table: "CreateQuestionMultipleChoiceAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMultipleChoices",
                table: "CreateQuestionMultipleChoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMathematicVariables",
                table: "CreateQuestionMathematicVariables",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMathematics",
                table: "CreateQuestionMathematics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionDistributeAnswers",
                table: "CreateQuestionDistributeAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionDistributes",
                table: "CreateQuestionDistributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_CreateSets_CreateSetId",
                        column: x => x.CreateSetId,
                        principalTable: "CreateSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRight = table.Column<bool>(type: "bit", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestionAnswers_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestionAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeQuestionsMathematicResolved",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<double>(type: "float", nullable: false),
                    Digits = table.Column<int>(type: "int", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeQuestionsMathematicResolved", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestionsMathematicResolved_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeUsers_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionAnswers_ChallengeId",
                table: "ChallengeQuestionAnswers",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionAnswers_UserId",
                table: "ChallengeQuestionAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionsMathematicResolved_ChallengeId",
                table: "ChallengeQuestionsMathematicResolved",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_CreateSetId",
                table: "Challenges",
                column: "CreateSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_OwnerId",
                table: "Challenges",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeUsers_ChallengeId",
                table: "ChallengeUsers",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeUsers_UserId",
                table: "ChallengeUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionDistributeAnswers_CreateQuestionDistributes_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswers",
                column: "QuestionDistributeId",
                principalTable: "CreateQuestionDistributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionDistributes_CreateSets_SetId",
                table: "CreateQuestionDistributes",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMathematics_CreateSets_SetId",
                table: "CreateQuestionMathematics",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMathematicVariables_CreateQuestionMathematics_QuestionMathematicId",
                table: "CreateQuestionMathematicVariables",
                column: "QuestionMathematicId",
                principalTable: "CreateQuestionMathematics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMultipleChoiceAnswers_CreateQuestionMultipleChoices_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswers",
                column: "QuestionMultipleChoiceId",
                principalTable: "CreateQuestionMultipleChoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMultipleChoices_CreateSets_SetId",
                table: "CreateQuestionMultipleChoices",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionOpenQuestions_CreateSets_SetId",
                table: "CreateQuestionOpenQuestions",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionTextFields_CreateSets_SetId",
                table: "CreateQuestionTextFields",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionTrueFalses_CreateSets_SetId",
                table: "CreateQuestionTrueFalses",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionWords_CreateSets_SetId",
                table: "CreateQuestionWords",
                column: "SetId",
                principalTable: "CreateSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateSets_Users_CreatedById",
                table: "CreateSets",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateSets_Users_ModifiedById",
                table: "CreateSets",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionDistributeAnswers_CreateQuestionDistributes_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionDistributes_CreateSets_SetId",
                table: "CreateQuestionDistributes");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMathematics_CreateSets_SetId",
                table: "CreateQuestionMathematics");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMathematicVariables_CreateQuestionMathematics_QuestionMathematicId",
                table: "CreateQuestionMathematicVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMultipleChoiceAnswers_CreateQuestionMultipleChoices_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionMultipleChoices_CreateSets_SetId",
                table: "CreateQuestionMultipleChoices");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionOpenQuestions_CreateSets_SetId",
                table: "CreateQuestionOpenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionTextFields_CreateSets_SetId",
                table: "CreateQuestionTextFields");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionTrueFalses_CreateSets_SetId",
                table: "CreateQuestionTrueFalses");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQuestionWords_CreateSets_SetId",
                table: "CreateQuestionWords");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateSets_Users_CreatedById",
                table: "CreateSets");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateSets_Users_ModifiedById",
                table: "CreateSets");

            migrationBuilder.DropTable(
                name: "ChallengeQuestionAnswers");

            migrationBuilder.DropTable(
                name: "ChallengeQuestionsMathematicResolved");

            migrationBuilder.DropTable(
                name: "ChallengeUsers");

            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateSets",
                table: "CreateSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionWords",
                table: "CreateQuestionWords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionTrueFalses",
                table: "CreateQuestionTrueFalses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionTextFields",
                table: "CreateQuestionTextFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionOpenQuestions",
                table: "CreateQuestionOpenQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMultipleChoices",
                table: "CreateQuestionMultipleChoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMultipleChoiceAnswers",
                table: "CreateQuestionMultipleChoiceAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMathematicVariables",
                table: "CreateQuestionMathematicVariables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionMathematics",
                table: "CreateQuestionMathematics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionDistributes",
                table: "CreateQuestionDistributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQuestionDistributeAnswers",
                table: "CreateQuestionDistributeAnswers");

            migrationBuilder.RenameTable(
                name: "CreateSets",
                newName: "CreateSet");

            migrationBuilder.RenameTable(
                name: "CreateQuestionWords",
                newName: "CreateQuestionWord");

            migrationBuilder.RenameTable(
                name: "CreateQuestionTrueFalses",
                newName: "CreateQuestionTrueFalse");

            migrationBuilder.RenameTable(
                name: "CreateQuestionTextFields",
                newName: "CreateQuestionTextField");

            migrationBuilder.RenameTable(
                name: "CreateQuestionOpenQuestions",
                newName: "CreateQuestionOpenQuestion");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMultipleChoices",
                newName: "CreateQuestionMultipleChoice");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMultipleChoiceAnswers",
                newName: "CreateQuestionMultipleChoiceAnswer");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMathematicVariables",
                newName: "CreateQuestionMathematicVariable");

            migrationBuilder.RenameTable(
                name: "CreateQuestionMathematics",
                newName: "CreateQuestionMathematic");

            migrationBuilder.RenameTable(
                name: "CreateQuestionDistributes",
                newName: "CreateQuestionDistribute");

            migrationBuilder.RenameTable(
                name: "CreateQuestionDistributeAnswers",
                newName: "CreateQuestionDistributeAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_CreateSets_ModifiedById",
                table: "CreateSet",
                newName: "IX_CreateSet_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_CreateSets_CreatedById",
                table: "CreateSet",
                newName: "IX_CreateSet_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionWords_SetId",
                table: "CreateQuestionWord",
                newName: "IX_CreateQuestionWord_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionTrueFalses_SetId",
                table: "CreateQuestionTrueFalse",
                newName: "IX_CreateQuestionTrueFalse_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionTextFields_SetId",
                table: "CreateQuestionTextField",
                newName: "IX_CreateQuestionTextField_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionOpenQuestions_SetId",
                table: "CreateQuestionOpenQuestion",
                newName: "IX_CreateQuestionOpenQuestion_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMultipleChoices_SetId",
                table: "CreateQuestionMultipleChoice",
                newName: "IX_CreateQuestionMultipleChoice_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMultipleChoiceAnswers_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswer",
                newName: "IX_CreateQuestionMultipleChoiceAnswer_QuestionMultipleChoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMathematicVariables_QuestionMathematicId",
                table: "CreateQuestionMathematicVariable",
                newName: "IX_CreateQuestionMathematicVariable_QuestionMathematicId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionMathematics_SetId",
                table: "CreateQuestionMathematic",
                newName: "IX_CreateQuestionMathematic_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionDistributes_SetId",
                table: "CreateQuestionDistribute",
                newName: "IX_CreateQuestionDistribute_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQuestionDistributeAnswers_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswer",
                newName: "IX_CreateQuestionDistributeAnswer_QuestionDistributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateSet",
                table: "CreateSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionWord",
                table: "CreateQuestionWord",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionTrueFalse",
                table: "CreateQuestionTrueFalse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionTextField",
                table: "CreateQuestionTextField",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionOpenQuestion",
                table: "CreateQuestionOpenQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMultipleChoice",
                table: "CreateQuestionMultipleChoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMultipleChoiceAnswer",
                table: "CreateQuestionMultipleChoiceAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMathematicVariable",
                table: "CreateQuestionMathematicVariable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionMathematic",
                table: "CreateQuestionMathematic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionDistribute",
                table: "CreateQuestionDistribute",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQuestionDistributeAnswer",
                table: "CreateQuestionDistributeAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionDistribute_CreateSet_SetId",
                table: "CreateQuestionDistribute",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionDistributeAnswer_CreateQuestionDistribute_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswer",
                column: "QuestionDistributeId",
                principalTable: "CreateQuestionDistribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMathematic_CreateSet_SetId",
                table: "CreateQuestionMathematic",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMathematicVariable_CreateQuestionMathematic_QuestionMathematicId",
                table: "CreateQuestionMathematicVariable",
                column: "QuestionMathematicId",
                principalTable: "CreateQuestionMathematic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMultipleChoice_CreateSet_SetId",
                table: "CreateQuestionMultipleChoice",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionMultipleChoiceAnswer_CreateQuestionMultipleChoice_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswer",
                column: "QuestionMultipleChoiceId",
                principalTable: "CreateQuestionMultipleChoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionOpenQuestion_CreateSet_SetId",
                table: "CreateQuestionOpenQuestion",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionTextField_CreateSet_SetId",
                table: "CreateQuestionTextField",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionTrueFalse_CreateSet_SetId",
                table: "CreateQuestionTrueFalse",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQuestionWord_CreateSet_SetId",
                table: "CreateQuestionWord",
                column: "SetId",
                principalTable: "CreateSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateSet_Users_CreatedById",
                table: "CreateSet",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateSet_Users_ModifiedById",
                table: "CreateSet",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
