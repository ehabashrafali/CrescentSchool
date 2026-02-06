using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrescentSchool.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifyReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicQuranRecitationRules",
                table: "StudentMonthlyReports");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "StudentMonthlyReports");

            migrationBuilder.DropColumn(
                name: "IslamicStudiesBooks",
                table: "StudentMonthlyReports");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "StudentMonthlyReports");

            migrationBuilder.RenameColumn(
                name: "TajweedRules",
                table: "StudentMonthlyReports",
                newName: "ReadingGrade");

            migrationBuilder.AlterColumn<int>(
                name: "Reading",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "QuranComments",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Memorization",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IslamicStudiesTopics",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "IslamicStudiesProgress",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IslamicStudiesComments",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "BasicQuranRecitationRulesProgress",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemorizationGrade",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TajweedRulesProgress",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BasicQuranRecitationRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quranRecitationTopic = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicQuranRecitationRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IslamicStudiesBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Book = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslamicStudiesBook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tajweed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TajweedRule = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tajweed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicQuranRecitationRuleStudentMonthlyReport",
                columns: table => new
                {
                    BasicQuranRecitationRulesId = table.Column<int>(type: "integer", nullable: false),
                    StudentMonthlyReportsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicQuranRecitationRuleStudentMonthlyReport", x => new { x.BasicQuranRecitationRulesId, x.StudentMonthlyReportsId });
                    table.ForeignKey(
                        name: "FK_BasicQuranRecitationRuleStudentMonthlyReport_BasicQuranReci~",
                        column: x => x.BasicQuranRecitationRulesId,
                        principalTable: "BasicQuranRecitationRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasicQuranRecitationRuleStudentMonthlyReport_StudentMonthly~",
                        column: x => x.StudentMonthlyReportsId,
                        principalTable: "StudentMonthlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IslamicStudiesBookStudentMonthlyReport",
                columns: table => new
                {
                    IslamicStudiesBooksId = table.Column<int>(type: "integer", nullable: false),
                    StudentMonthlyReportsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslamicStudiesBookStudentMonthlyReport", x => new { x.IslamicStudiesBooksId, x.StudentMonthlyReportsId });
                    table.ForeignKey(
                        name: "FK_IslamicStudiesBookStudentMonthlyReport_IslamicStudiesBook_I~",
                        column: x => x.IslamicStudiesBooksId,
                        principalTable: "IslamicStudiesBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IslamicStudiesBookStudentMonthlyReport_StudentMonthlyReport~",
                        column: x => x.StudentMonthlyReportsId,
                        principalTable: "StudentMonthlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentMonthlyReportTajweed",
                columns: table => new
                {
                    StudentMonthlyReportsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TajweedRulesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMonthlyReportTajweed", x => new { x.StudentMonthlyReportsId, x.TajweedRulesId });
                    table.ForeignKey(
                        name: "FK_StudentMonthlyReportTajweed_StudentMonthlyReports_StudentMo~",
                        column: x => x.StudentMonthlyReportsId,
                        principalTable: "StudentMonthlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentMonthlyReportTajweed_Tajweed_TajweedRulesId",
                        column: x => x.TajweedRulesId,
                        principalTable: "Tajweed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicQuranRecitationRuleStudentMonthlyReport_StudentMonthly~",
                table: "BasicQuranRecitationRuleStudentMonthlyReport",
                column: "StudentMonthlyReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_IslamicStudiesBookStudentMonthlyReport_StudentMonthlyReport~",
                table: "IslamicStudiesBookStudentMonthlyReport",
                column: "StudentMonthlyReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMonthlyReportTajweed_TajweedRulesId",
                table: "StudentMonthlyReportTajweed",
                column: "TajweedRulesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicQuranRecitationRuleStudentMonthlyReport");

            migrationBuilder.DropTable(
                name: "IslamicStudiesBookStudentMonthlyReport");

            migrationBuilder.DropTable(
                name: "StudentMonthlyReportTajweed");

            migrationBuilder.DropTable(
                name: "BasicQuranRecitationRule");

            migrationBuilder.DropTable(
                name: "IslamicStudiesBook");

            migrationBuilder.DropTable(
                name: "Tajweed");

            migrationBuilder.DropColumn(
                name: "BasicQuranRecitationRulesProgress",
                table: "StudentMonthlyReports");

            migrationBuilder.DropColumn(
                name: "MemorizationGrade",
                table: "StudentMonthlyReports");

            migrationBuilder.DropColumn(
                name: "TajweedRulesProgress",
                table: "StudentMonthlyReports");

            migrationBuilder.RenameColumn(
                name: "ReadingGrade",
                table: "StudentMonthlyReports",
                newName: "TajweedRules");

            migrationBuilder.AlterColumn<int>(
                name: "Reading",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuranComments",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Memorization",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IslamicStudiesTopics",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IslamicStudiesProgress",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IslamicStudiesComments",
                table: "StudentMonthlyReports",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasicQuranRecitationRules",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IslamicStudiesBooks",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "StudentMonthlyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
