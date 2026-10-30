using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingupcommingsessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoomMeeting",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ZoomMeeting",
                table: "Instructors");

            migrationBuilder.CreateTable(
                name: "UpCommingSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ZoomMeeting = table.Column<string>(type: "text", nullable: false),
                    StudentName = table.Column<string>(type: "text", nullable: false),
                    InstructorName = table.Column<string>(type: "text", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstJoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLeftAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalDurationInSeconds = table.Column<int>(type: "integer", nullable: false),
                    ZoomMeetingId = table.Column<string>(type: "text", nullable: false),
                    ZoomMeetingUuid = table.Column<string>(type: "text", nullable: false),
                    ZoomJoinUrl = table.Column<string>(type: "text", nullable: false),
                    IsCurrentlyJoined = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpCommingSessions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpCommingSessions");

            migrationBuilder.AddColumn<string>(
                name: "ZoomMeeting",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZoomMeeting",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
