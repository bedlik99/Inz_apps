using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class PDIcorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RegisteredUserItems");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "RegisteredUserItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
