using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class ExecutedCommandEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropColumn(
                name: "RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.AlterColumn<int>(
                name: "LaboratoryId",
                table: "LaboratoryRequirementsItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExecutedCommandsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisteredUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedCommandsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutedCommandsItems_RegisteredUserItems_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "RegisteredUserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutedCommandsItems_RegisteredUserId",
                table: "ExecutedCommandsItems",
                column: "RegisteredUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropTable(
                name: "ExecutedCommandsItems");

            migrationBuilder.AlterColumn<int>(
                name: "LaboratoryId",
                table: "LaboratoryRequirementsItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                column: "RegisteredUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                column: "RegisteredUserId",
                principalTable: "RegisteredUserItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
