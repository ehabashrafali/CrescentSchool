using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quranRecitationTopic",
                table: "BasicQuranRecitationRule",
                newName: "QuranRecitationTopic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuranRecitationTopic",
                table: "BasicQuranRecitationRule",
                newName: "quranRecitationTopic");
        }
    }
}
