using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class RegisteredUserChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoWarning",
                table: "RegisteredUserItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaboratoryRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaboratoryRequirements_LaboratoryItems_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "LaboratoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryRequirements_LaboratoryId",
                table: "LaboratoryRequirements",
                column: "LaboratoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaboratoryRequirements");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RegisteredUserItems");

            migrationBuilder.DropColumn(
                name: "NoWarning",
                table: "RegisteredUserItems");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "RegisteredUserItems");
        }
    }
}
