namespace CrescentSchool.BLL.Services
{
    public interface IZoomService
    {
        Task<string> CreateMeetingAsync(string topic, DateTime startTime, CancellationToken cancellationToken = default);
    }
}
