using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations
{
    internal class BasicQuranRecitationRuleConfiguration : IEntityTypeConfiguration<BasicQuranRecitationRule>
    {
        public void Configure(EntityTypeBuilder<BasicQuranRecitationRule> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(b => b.QuranRecitationTopic)
                   .HasConversion<int>();
            builder.HasMany(b => b.StudentMonthlyReports)
                   .WithMany(r => r.BasicQuranRecitationRules)
                   .UsingEntity(j => j.ToTable("StudentMonthlyReportBasicQuranRecitationRules"));
        }
    }
}
