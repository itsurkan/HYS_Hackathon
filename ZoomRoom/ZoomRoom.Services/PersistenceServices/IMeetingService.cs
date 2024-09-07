using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IMeetingService
{
    Task<Meeting> CreateMeetingAsync(Meeting meeting);
    Task<Meeting> UpdateMeetingAsync(Meeting meeting);
    Task DeleteMeetingAsync(long meetingId);
    Task<Meeting?> GetMeetingByIdAsync(long meetingId);
    Task<List<Meeting>> GetAllMeetingsAsync();
}
