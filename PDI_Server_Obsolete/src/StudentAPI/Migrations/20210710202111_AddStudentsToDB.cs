using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAPI.Migrations
{
    public partial class AddStudentsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexNum = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    IsCheater = table.Column<bool>(type: "bit", nullable: false),
                    AccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Mark = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentItems");
        }
    }
}
