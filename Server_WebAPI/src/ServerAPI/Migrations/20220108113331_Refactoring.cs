using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "LaboratoryRequirementsItems");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "RoleItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueCode",
                table: "RegisteredUserItems",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "LaboratoryId",
                table: "RegisteredUserItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RegisteredUserItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LabOrganizer",
                table: "LaboratoryItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LabName",
                table: "LaboratoryItems",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "EmployeeItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "EmployeeItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems",
                column: "RoleId",
                principalTable: "RoleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "RoleItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueCode",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<int>(
                name: "LaboratoryId",
                table: "RegisteredUserItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RegisteredUserItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "LaboratoryRequirementsItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "LabOrganizer",
                table: "LaboratoryItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LabName",
                table: "LaboratoryItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "EmployeeItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "EmployeeItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeItems_RoleItems_RoleId",
                table: "EmployeeItems",
                column: "RoleId",
                principalTable: "RoleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredUserItems_LaboratoryItems_LaboratoryId",
                table: "RegisteredUserItems",
                column: "LaboratoryId",
                principalTable: "LaboratoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
