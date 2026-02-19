using CrescentSchool.DAL.Entities;
using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class WeeklyAppointmentConfiguration : IEntityTypeConfiguration<WeeklyAppointment>
{
    public void Configure(EntityTypeBuilder<WeeklyAppointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Day)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.Time)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasOne(x => x.Student)
               .WithMany(s => s.WeeklyAppointments)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
