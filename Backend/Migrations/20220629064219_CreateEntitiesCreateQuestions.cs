using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class CreateEntitiesCreateQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreateSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectMain = table.Column<int>(type: "int", nullable: false),
                    SubjectSecond = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetPolicy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateSet_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreateSet_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionDistribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionDistribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionDistribute_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionMathematic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Digits = table.Column<int>(type: "int", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionMathematic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionMathematic_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionMultipleChoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionMultipleChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionMultipleChoice_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionOpenQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionOpenQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionOpenQuestion_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionTextField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionTextField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionTextField_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionTrueFalse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<bool>(type: "bit", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionTrueFalse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionTrueFalse_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageSubjectMain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageSubjectSecond = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionWord_CreateSet_SetId",
                        column: x => x.SetId,
                        principalTable: "CreateSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionDistributeAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeftSide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightSide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionDistributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionDistributeAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionDistributeAnswer_CreateQuestionDistribute_QuestionDistributeId",
                        column: x => x.QuestionDistributeId,
                        principalTable: "CreateQuestionDistribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionMathematicVariable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Display = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Min = table.Column<double>(type: "float", nullable: false),
                    Max = table.Column<double>(type: "float", nullable: false),
                    Digits = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<double>(type: "float", nullable: false),
                    QuestionMathematicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionMathematicVariable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionMathematicVariable_CreateQuestionMathematic_QuestionMathematicId",
                        column: x => x.QuestionMathematicId,
                        principalTable: "CreateQuestionMathematic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateQuestionMultipleChoiceAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRight = table.Column<bool>(type: "bit", nullable: false),
                    QuestionMultipleChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQuestionMultipleChoiceAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQuestionMultipleChoiceAnswer_CreateQuestionMultipleChoice_QuestionMultipleChoiceId",
                        column: x => x.QuestionMultipleChoiceId,
                        principalTable: "CreateQuestionMultipleChoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionDistribute_SetId",
                table: "CreateQuestionDistribute",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionDistributeAnswer_QuestionDistributeId",
                table: "CreateQuestionDistributeAnswer",
                column: "QuestionDistributeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionMathematic_SetId",
                table: "CreateQuestionMathematic",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionMathematicVariable_QuestionMathematicId",
                table: "CreateQuestionMathematicVariable",
                column: "QuestionMathematicId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionMultipleChoice_SetId",
                table: "CreateQuestionMultipleChoice",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionMultipleChoiceAnswer_QuestionMultipleChoiceId",
                table: "CreateQuestionMultipleChoiceAnswer",
                column: "QuestionMultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionOpenQuestion_SetId",
                table: "CreateQuestionOpenQuestion",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionTextField_SetId",
                table: "CreateQuestionTextField",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionTrueFalse_SetId",
                table: "CreateQuestionTrueFalse",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQuestionWord_SetId",
                table: "CreateQuestionWord",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateSet_CreatedById",
                table: "CreateSet",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CreateSet_ModifiedById",
                table: "CreateSet",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreateQuestionDistributeAnswer");

            migrationBuilder.DropTable(
                name: "CreateQuestionMathematicVariable");

            migrationBuilder.DropTable(
                name: "CreateQuestionMultipleChoiceAnswer");

            migrationBuilder.DropTable(
                name: "CreateQuestionOpenQuestion");

            migrationBuilder.DropTable(
                name: "CreateQuestionTextField");

            migrationBuilder.DropTable(
                name: "CreateQuestionTrueFalse");

            migrationBuilder.DropTable(
                name: "CreateQuestionWord");

            migrationBuilder.DropTable(
                name: "CreateQuestionDistribute");

            migrationBuilder.DropTable(
                name: "CreateQuestionMathematic");

            migrationBuilder.DropTable(
                name: "CreateQuestionMultipleChoice");

            migrationBuilder.DropTable(
                name: "CreateSet");
        }
    }
}
