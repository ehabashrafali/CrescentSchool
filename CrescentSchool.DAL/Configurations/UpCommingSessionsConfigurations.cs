using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL.Configurations;

public class UpCommingSessionsConfigurations : IEntityTypeConfiguration<UpCommingSessions>
{
    public void Configure(EntityTypeBuilder<UpCommingSessions> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(s => new
        {
            s.SessionDateTime,
            s.StudentId,
            s.InstructorId
        }).IsUnique();

        builder.Property(s => s.SessionDateTime)
               .IsRequired();

        builder.Property(s => s.StudentId)
               .IsRequired();

        builder.Property(s => s.InstructorId)
               .IsRequired();

        builder.Property(s => s.StudentName)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(s => s.InstructorName)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(s => s.ZoomMeeting)
               .HasMaxLength(2048)
               .IsRequired(false);

        builder.Property(s => s.ZoomMeetingId)
               .IsRequired();

        builder.Property(s => s.ZoomMeetingUuid)
               .HasMaxLength(255)
               .IsRequired(false);

        builder.Property(s => s.ZoomJoinUrl)
               .HasMaxLength(2048)
               .IsRequired(false);

        builder.Property(s => s.FirstJoinedAt)
               .IsRequired(false);

        builder.Property(s => s.LastLeftAt)
               .IsRequired(false);

        builder.Property(s => s.TotalDuration)
               .HasDefaultValue(0)
               .IsRequired();

        builder.Property(s => s.IsJoined)
               .HasDefaultValue(false)
               .IsRequired();

        builder.Property(s => s.Canceled)
            .HasDefaultValue(null);

        builder.Property(s => s.IsRescheduled)
            .HasDefaultValue(false);
    }
}