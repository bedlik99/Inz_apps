using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class RoleRestored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "EmployeeItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                column: "RegisteredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_RoleId",
                table: "EmployeeItems",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems",
                column: "RoleId",
                principalTable: "RoleItems",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropTable(
                name: "RoleItems");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropColumn(
                name: "RegisteredUserId",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "EmployeeItems");
        }
    }
}
