using CrescentSchool.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace CrescentSchool.BLL.Services;

public sealed class ZoomWebhookInbox(IZoomWebhookInboxRepository zoomWebhookInboxRepository) : IZoomWebhookInbox
{
    public Task<bool> SaveIfNewAsync(string eventName, long meetingId, string meetingUuid, string rawBody, CancellationToken cancellationToken)
    {
        var bodyHash = SHA256.HashData(Encoding.UTF8.GetBytes(rawBody));

        return zoomWebhookInboxRepository.InsertIfNewAsync(
            eventName,
            meetingId,
            meetingUuid,
            bodyHash,
            rawBody,
            cancellationToken);
    }
}