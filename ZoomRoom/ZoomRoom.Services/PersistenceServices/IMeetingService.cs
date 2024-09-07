using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IMeetingService
{
    Task<Meeting> CreateMeetingAsync(Meeting meeting);
    Task<Meeting> UpdateMeetingAsync(Meeting meeting);
    Task DeleteMeetingAsync(int meetingId);
    Task<Meeting?> GetMeetingByIdAsync(int meetingId);
    Task<List<Meeting>> GetAllMeetingsAsync();
}
