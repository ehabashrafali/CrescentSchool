namespace CrescentSchool.BLL.Services
{
    public interface IZoomService
    {
        Task<ZoomMeetingResponse> CreateMeetingAsync(string hostUserId, string topic, DateTime startTime, CancellationToken cancellationToken = default);
        Task MarkAsJoined(string meetingUuid, string joinTime, CancellationToken cancellationToken = default);
        Task MarkAsLeft(string meetingUuid, string leftTime, CancellationToken cancellationToken = default);
    }
}
