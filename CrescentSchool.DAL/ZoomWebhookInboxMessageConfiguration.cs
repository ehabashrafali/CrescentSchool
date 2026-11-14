using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrescentSchool.DAL;

internal sealed class ZoomWebhookInboxMessageConfiguration
    : IEntityTypeConfiguration<ZoomWebhookInboxMessage>
{
    public void Configure(EntityTypeBuilder<ZoomWebhookInboxMessage> builder)
    {
        builder.HasKey(message => message.Id);

        builder.Property(message => message.EventName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(message => message.MeetingUUID)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(message => message.MeetingId)
            .IsRequired();

        builder.Property(message => message.Payload)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(message => message.BodySha256)
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(message => message.ReceivedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(message => message.ProcessedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(message => message.BodySha256)
            .IsUnique()
            .HasDatabaseName("uq_zoom_webhook_inbox_body_sha256");

        builder.HasIndex(message => message.MeetingId)
            .HasDatabaseName("ix_zoom_webhook_inbox_meeting_id");
    }
}