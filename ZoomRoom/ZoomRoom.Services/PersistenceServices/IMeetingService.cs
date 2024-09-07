using System.Collections;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Implementation.Repositories;

namespace ZoomRoom.Services.PersistenceServices;

public interface IMeetingService
{
    Task<Meeting> CreateMeetingAsync(Meeting meeting);
    Task<Meeting> UpdateMeetingAsync(Meeting meeting);
    Task DeleteMeetingAsync(long meetingId);
    Task<int> DeleteMeetingAsync(string name);
    Task<Meeting?> GetMeetingByIdAsync(long meetingId);
    Task<List<Meeting>> GetAllMeetingsAsync();
    Task<IEnumerable<Meeting>> GetMeetingsToStartAsync(DateTime utcNow);
}
