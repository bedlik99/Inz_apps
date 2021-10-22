using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class EmailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexNum",
                table: "RegisteredUserItems");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "RegisteredUserItems");

            migrationBuilder.AddColumn<string>(
                name: "IndexNum",
                table: "RegisteredUserItems",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }
    }
}
