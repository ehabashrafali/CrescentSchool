using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changezoommeetingid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        ALTER TABLE "UpCommingSessions"
        ALTER COLUMN "ZoomMeetingId" TYPE bigint
        USING "ZoomMeetingId"::bigint;
        """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        ALTER TABLE "UpCommingSessions"
        ALTER COLUMN "ZoomMeetingId" TYPE text
        USING "ZoomMeetingId"::text;
        """);
        }
    }
}
