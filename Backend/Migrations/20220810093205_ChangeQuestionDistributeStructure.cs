using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class ChangeQuestionDistributeStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LeftSideId",
                table: "CreateQuestionDistributeAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RightSideId",
                table: "CreateQuestionDistributeAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftSideId",
                table: "CreateQuestionDistributeAnswers");

            migrationBuilder.DropColumn(
                name: "RightSideId",
                table: "CreateQuestionDistributeAnswers");
        }
    }
}
