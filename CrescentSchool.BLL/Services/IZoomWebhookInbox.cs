namespace CrescentSchool.BLL.Services;

public interface IZoomWebhookInbox
{
    Task<bool> SaveIfNewAsync(string eventName, string meetingUuid, string rawBody, CancellationToken cancellationToken);
}
