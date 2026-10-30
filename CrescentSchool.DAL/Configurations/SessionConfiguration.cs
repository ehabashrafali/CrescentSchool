using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Date)
               .IsRequired();

        builder.Property(s => s.StudentStatus)
               .IsRequired();

        builder.Property(s => s.InstructorStatus)
               .IsRequired();

        builder.Property(s => s.Duration)
               .IsRequired();

        builder.Property(s => s.IsDeleted)
               .HasDefaultValue(false);

        builder.Property(s => s.TotalDurationInSeconds)
               .HasDefaultValue(0);

        builder.Property(s => s.ZoomJoinUrl)
               .HasMaxLength(255)
               .IsRequired(false);

        builder.Property(s => s.FirstJoinedAt)
               .IsRequired(false);

        builder.Property(s => s.LastLeftAt)
               .IsRequired(false);

        builder.HasOne(s => s.Student)
               .WithMany()
               .HasForeignKey(s => s.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Instructor)
               .WithMany()
               .HasForeignKey(s => s.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);



        builder.ToTable("Sessions");
    }
}
