using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnz.Migrations
{
    public partial class DrawSegmentsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrawCanvasStoragePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawCanvasStoragePoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrawCanvasStorages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrawPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawCanvasStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrawCanvasStorages_DrawCanvasStoragePoints_FromPositionId",
                        column: x => x.FromPositionId,
                        principalTable: "DrawCanvasStoragePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawCanvasStorages_DrawCanvasStoragePoints_ToPositionId",
                        column: x => x.ToPositionId,
                        principalTable: "DrawCanvasStoragePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrawCanvasStorages_DrawPages_DrawPageId",
                        column: x => x.DrawPageId,
                        principalTable: "DrawPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrawCanvasStorages_DrawPageId",
                table: "DrawCanvasStorages",
                column: "DrawPageId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawCanvasStorages_FromPositionId",
                table: "DrawCanvasStorages",
                column: "FromPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawCanvasStorages_ToPositionId",
                table: "DrawCanvasStorages",
                column: "ToPositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrawCanvasStorages");

            migrationBuilder.DropTable(
                name: "DrawCanvasStoragePoints");
        }
    }
}
