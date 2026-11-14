using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class altercancelcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "UpCommingSessions");

            migrationBuilder.AddColumn<int>(
                name: "Canceled",
                table: "UpCommingSessions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "UpCommingSessions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "UpCommingSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
