using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Instructors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Students",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Instructors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
