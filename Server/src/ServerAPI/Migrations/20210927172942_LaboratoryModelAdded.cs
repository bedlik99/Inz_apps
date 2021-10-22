using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class LaboratoryModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaboratoryId",
                table: "RegisteredUserItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaboratoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LabOrganizer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredUserItems_LaboratoryId",
                table: "RegisteredUserItems",
                column: "LaboratoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems");

            migrationBuilder.DropTable(
                name: "LaboratoryItems");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredUserItems_LaboratoryId",
                table: "RegisteredUserItems");

            migrationBuilder.DropColumn(
                name: "LaboratoryId",
                table: "RegisteredUserItems");
        }
    }
}
