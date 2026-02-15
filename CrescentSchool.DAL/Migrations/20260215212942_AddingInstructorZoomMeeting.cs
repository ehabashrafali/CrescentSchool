using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingInstructorZoomMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReadingGrade",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ZoomMeeting",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoomMeeting",
                table: "Instructors");

            migrationBuilder.AlterColumn<int>(
                name: "ReadingGrade",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
