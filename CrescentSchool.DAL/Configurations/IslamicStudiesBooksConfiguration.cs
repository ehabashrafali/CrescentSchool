using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations
{
    internal class IslamicStudiesBooksConfiguration : IEntityTypeConfiguration<IslamicStudiesBook>
    {
        public void Configure(EntityTypeBuilder<IslamicStudiesBook> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(i => i.Book)
                     .HasConversion<int>();

            builder.HasMany(i => i.StudentMonthlyReports)
                   .WithMany(r => r.IslamicStudiesBooks)
                   .UsingEntity(j => j.ToTable("StudentMonthlyReportIslamicStudiesBooks"));
        }
    }
}
