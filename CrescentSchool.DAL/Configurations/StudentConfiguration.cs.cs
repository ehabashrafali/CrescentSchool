using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.ZoomMeeting)
            .HasMaxLength(500);

        builder.Property(s => s.Fees)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder
            .HasOne(s => s.Instructor)
            .WithMany(i => i.Students)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.StudentMonthlyReports)
            .WithOne(r => r.Student)
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Sessions)
            .WithOne(sess => sess.Student)
            .HasForeignKey(sess => sess.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity(j => j.ToTable("StudentCourses"));

        builder.ToTable("Students");
    }
}