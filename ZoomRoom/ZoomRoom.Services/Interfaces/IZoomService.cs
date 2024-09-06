using ZoomRoom.Domain.Requests;
using ZoomRoom.Domain.Responses;

namespace ZoomRoom.Services.Interfaces
{
    public interface IZoomService
    {
        Task<string> GetAccessTokenAsync();
        Task<string> CreateMeetingAsync(string accessToken, MeetingBodyRequest request);
        Task<List<MeetingResponse>> GetUpcomingMeetingsAsync(string accessToken);
        Task<bool> DeleteMeetingAsync(string accessToken, string meetingId);
        Task<bool> UpdateMeetingAsync(string accessToken, string meetingId, MeetingBodyRequest requestBody);
    }
}
