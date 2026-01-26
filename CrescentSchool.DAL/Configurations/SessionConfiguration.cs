using CrescentSchool.Models;
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
