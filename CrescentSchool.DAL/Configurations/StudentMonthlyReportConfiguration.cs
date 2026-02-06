using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class StudentMonthlyReportConfiguration
    : IEntityTypeConfiguration<StudentMonthlyReport>
{
    public void Configure(EntityTypeBuilder<StudentMonthlyReport> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Date)
            .HasColumnType("date");

        builder.Property(r => r.Memorization)
               .HasConversion<int>();

        builder.Property(r => r.Reading)
                .HasConversion<int>();

        builder.Property(r => r.NoOfMemorizationAyah);

        builder.Property(r => r.NoOfReadingAyah);

        builder.Property(r => r.MemorizationGrade)
               .HasConversion<int>();


        builder.Property(r => r.ReadingGrade)
               .HasConversion<int>();

        builder.Property(r => r.QuranComments)
               .HasMaxLength(1000);

        builder.Property(r => r.IslamicStudiesComments)
               .HasMaxLength(1000);

        builder.Property(r => r.IslamicStudiesTopics)
               .HasMaxLength(500);

        builder.Property(r => r.IslamicStudiesProgress)
               .HasConversion<int>();

        builder.Property(r => r.BasicQuranRecitationRulesProgress)
               .HasConversion<int>();

        builder.Property(r => r.TajweedRulesProgress)
               .HasConversion<int>();

        builder
            .HasOne(r => r.Student)
            .WithMany(s => s.StudentMonthlyReports)
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("StudentMonthlyReports");
    }
}
