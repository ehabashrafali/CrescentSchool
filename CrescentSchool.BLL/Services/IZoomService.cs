namespace CrescentSchool.BLL.Services
{
    public interface IZoomService
    {
        Task<ZoomMeetingResponse> CreateMeetingAsync(string hostUserId, string topic, DateTime startTime, CancellationToken cancellationToken = default);
    }
}
