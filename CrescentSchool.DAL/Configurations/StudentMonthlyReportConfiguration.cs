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
            .IsRequired()
            .HasColumnType("date");

        builder.Property(r => r.Memorization)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.Reading)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.NoOfMemorizationAyah)
            .IsRequired();

        builder.Property(r => r.NoOfReadingAyah)
            .IsRequired();

        builder.Property(r => r.Grade)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.Progress)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.BasicQuranRecitationRules)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.TajweedRules)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.QuranComments)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(r => r.IslamicStudiesComments)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(r => r.IslamicStudiesTopics)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(r => r.IslamicStudiesBooks)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.IslamicStudiesProgress)
            .IsRequired()
            .HasConversion<int>();

        builder
            .HasOne(r => r.Student)
            .WithMany(s => s.StudentMonthlyReports)
            .HasForeignKey("StudentId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("StudentMonthlyReports");
    }
}
