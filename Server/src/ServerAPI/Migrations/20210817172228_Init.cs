using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisteredUserItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexNum = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    UniqueCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredUserItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecordedEventItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistryContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisteredUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordedEventItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordedEventItems_RegisteredUserItems_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "RegisteredUserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordedEventItems_RegisteredUserId",
                table: "RecordedEventItems",
                column: "RegisteredUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordedEventItems");

            migrationBuilder.DropTable(
                name: "RegisteredUserItems");
        }
    }
}
