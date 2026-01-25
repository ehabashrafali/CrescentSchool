using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuditableInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Instructors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Instructors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Instructors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Instructors",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
