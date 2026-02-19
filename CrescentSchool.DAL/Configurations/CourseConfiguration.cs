using CrescentSchool.DAL.Entities;
using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.Description)
            .HasColumnName("Description")
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.PricePerHour)
            .IsRequired()
            .HasColumnType("decimal(26,9)");

        builder
            .HasMany(c => c.Students)
            .WithMany(s => s.Courses)
            .UsingEntity(j => j.ToTable("CourseStudents"));

        builder
            .HasMany(c => c.Instructors)
            .WithMany(i => i.Courses)
            .UsingEntity(j => j.ToTable("CourseInstructors"));
    }
}
