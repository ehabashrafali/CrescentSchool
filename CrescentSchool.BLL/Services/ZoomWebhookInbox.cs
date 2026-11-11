using CrescentSchool.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace CrescentSchool.BLL.Services;

public sealed class ZoomWebhookInbox(
    IZoomWebhookInboxRepository zoomWebhookInboxRepository) : IZoomWebhookInbox
{
    public Task<bool> SaveIfNewAsync(string eventName, string meetingUuid, string rawBody, CancellationToken cancellationToken)
    {
        var bodyHash = SHA256.HashData(Encoding.UTF8.GetBytes(rawBody));

        return zoomWebhookInboxRepository.InsertIfNewAsync(
            eventName,
            meetingUuid,
            bodyHash,
            rawBody,
            cancellationToken);
    }
}