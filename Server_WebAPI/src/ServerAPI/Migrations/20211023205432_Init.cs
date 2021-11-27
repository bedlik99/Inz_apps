using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "RoleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredUserItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoWarning = table.Column<bool>(type: "bit", nullable: false),
                    LaboratoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredUserItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "LaboratoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeItems_RoleItems_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryRequirementsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoryId = table.Column<int>(type: "int", nullable: true),
                    RegisteredUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryRequirementsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaboratoryRequirementsItems_LaboratoryItems_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "LaboratoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryRequirementsItems_RegisteredUserItems_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "RegisteredUserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecordedEventItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistryContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisteredUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordedEventItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordedEventItems_RegisteredUserItems_RegisteredUserId",
                        column: x => x.RegisteredUserId,
                        principalTable: "RegisteredUserItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeItems_RoleId",
                table: "EmployeeItems",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryRequirementsItems_LaboratoryId",
                table: "LaboratoryRequirementsItems",
                column: "LaboratoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryRequirementsItems_RegisteredUserId",
                table: "LaboratoryRequirementsItems",
                column: "RegisteredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordedEventItems_RegisteredUserId",
                table: "RecordedEventItems",
                column: "RegisteredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredUserItems_LaboratoryId",
                table: "RegisteredUserItems",
                column: "LaboratoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeItems");

            migrationBuilder.DropTable(
                name: "LaboratoryRequirementsItems");

            migrationBuilder.DropTable(
                name: "RecordedEventItems");

            migrationBuilder.DropTable(
                name: "RoleItems");

            migrationBuilder.DropTable(
                name: "RegisteredUserItems");

            migrationBuilder.DropTable(
                name: "LaboratoryItems");
        }
    }
}
