using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.ZoomMeeting)
            .HasMaxLength(500);

        builder.Property(i => i.Fees)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder
            .HasOne(i => i.User)
            .WithOne()
            .HasForeignKey<Instructor>(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(i => i.Students)
            .WithOne(s => s.Instructor)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(i => i.Sessions)
            .WithOne(s => s.Instructor)
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Instructors");
    }
}