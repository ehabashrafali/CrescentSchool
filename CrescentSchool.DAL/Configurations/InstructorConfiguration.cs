using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FirstName)
              .IsRequired()
              .HasColumnName("FirstName")
              .HasColumnType("nvarchar(200)");

        builder.Property(i => i.LastName)
                .IsRequired()
                .HasColumnName("LastName")
                .HasColumnType("nvarchar(200)");

        builder.Property(i => i.Email)
            .HasColumnName("email")
            .HasColumnType("nvarchar(200)");

        builder.Property(i => i.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasColumnType("nvarchar(20)");

        builder.Property(i => i.Country)
            .HasColumnName("Country")
            .HasColumnType("nvarchar(100)");

        builder.Property(i => i.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.HasMany(i => i.Sessions)
            .WithOne(s => s.Instructor)
            .HasForeignKey("InstructorId")
            .OnDelete(DeleteBehavior.Cascade);

    }
}
