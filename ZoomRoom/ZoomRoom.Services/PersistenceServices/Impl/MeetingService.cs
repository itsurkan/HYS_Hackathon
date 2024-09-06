using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices.Impl;

public class MeetingService(SqliteDbContext context)
{
    public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
    {
        context.Meetings.Add(meeting);
        await context.SaveChangesAsync();
        return meeting;
    }

    public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
    {
        context.Meetings.Update(meeting);
        await context.SaveChangesAsync();
        return meeting;
    }

    public async Task DeleteMeetingAsync(int meetingId)
    {
        var meeting = await context.Meetings.FindAsync(meetingId);
        if (meeting != null)
        {
            context.Meetings.Remove(meeting);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Meeting?> GetMeetingByIdAsync(int meetingId)
    {
        return await context.Meetings.FindAsync(meetingId);
    }

    public async Task<List<Meeting>> GetAllMeetingsAsync()
    {
        return await context.Meetings.ToListAsync();
    }
}
