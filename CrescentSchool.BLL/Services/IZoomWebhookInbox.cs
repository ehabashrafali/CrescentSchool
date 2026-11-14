namespace CrescentSchool.BLL.Services;

public interface IZoomWebhookInbox
{
    Task<bool> SaveIfNewAsync(string eventName, long meetingId, string meetingUuid, string rawBody, CancellationToken cancellationToken);
}
