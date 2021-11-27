using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class SolvingProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropColumn(
                name: "RegisteredUserId",
                table: "LaboratoryRequirementsItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                column: "RegisteredUserId",
                principalTable: "RegisteredUserItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
