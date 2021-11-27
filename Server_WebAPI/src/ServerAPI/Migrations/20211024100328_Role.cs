using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class Role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "EmployeeItems");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "RoleItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "RoleItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "EmployeeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
