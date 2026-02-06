using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations
{
    internal class TajweedConfiguration : IEntityTypeConfiguration<Tajweed>
    {
        public void Configure(EntityTypeBuilder<Tajweed> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(t => t.TajweedRule)
                   .HasConversion<int>();

            builder.HasMany(t => t.StudentMonthlyReports)
                   .WithMany(r => r.TajweedRules)
                   .UsingEntity(j => j.ToTable("StudentMonthlyReportTajweedRules"));
        }
    }
}
