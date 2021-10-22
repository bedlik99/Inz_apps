using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class DBRequirementsEntityadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirements_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaboratoryRequirements",
                table: "LaboratoryRequirements");

            migrationBuilder.RenameTable(
                name: "LaboratoryRequirements",
                newName: "LaboratoryRequirementsItems");

            migrationBuilder.RenameIndex(
                name: "IX_LaboratoryRequirements_LaboratoryId",
                table: "LaboratoryRequirementsItems",
                newName: "IX_LaboratoryRequirementsItems_LaboratoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaboratoryRequirementsItems",
                table: "LaboratoryRequirementsItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaboratoryRequirementsItems",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.RenameTable(
                name: "LaboratoryRequirementsItems",
                newName: "LaboratoryRequirements");

            migrationBuilder.RenameIndex(
                name: "IX_LaboratoryRequirementsItems_LaboratoryId",
                table: "LaboratoryRequirements",
                newName: "IX_LaboratoryRequirements_LaboratoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaboratoryRequirements",
                table: "LaboratoryRequirements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryRequirements_LaboratoryItems_LaboratoryId",
                table: "LaboratoryRequirements",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
