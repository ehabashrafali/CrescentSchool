using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.ParentName)
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .IsRequired();

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.Country)
            .HasMaxLength(100);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.Property(s => s.ZoomMeeting)
            .HasMaxLength(500);

        builder.Property(s => s.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");

        builder
            .HasOne(s => s.Parent)
            .WithMany(p => p.Students)
            .HasForeignKey("ParentId")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(s => s.StudentMonthlyReports)
            .WithOne(r => r.Student)
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Instructors)
            .WithMany(i => i.Students)
            .UsingEntity(j => j.ToTable("StudentInstructors"));

        builder
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity(j => j.ToTable("StudentCourses"));

        builder
            .HasMany(s => s.Sessions)
            .WithOne(sess => sess.Student)
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Students");
    }
}
