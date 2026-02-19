using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Country)
            .HasColumnName("Country")
            .HasColumnType("nvarchar(100)");

        builder.Property(i => i.ZoomMeeting)
            .HasColumnName("ZoomMeeting");

        builder.Property(i => i.Fees)
               .HasColumnType("decimal(18,2)")
               .HasDefaultValue(0.00m);

        builder
                .HasOne(i => i.User)
                .WithOne()
                .HasForeignKey<Instructor>(i => i.Id)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Sessions)
            .WithOne(s => s.Instructor)
            .HasForeignKey("InstructorId")
            .OnDelete(DeleteBehavior.Cascade);

    }
}
