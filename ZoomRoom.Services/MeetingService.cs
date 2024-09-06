using CMB.Persistence;
using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services;

public class MeetingService(SqliteDbContext context)
{
    public async Task<Meeting> CreateMeeting(Meeting meeting)
    {
        context.Meetings.Add(meeting);
        await context.SaveChangesAsync();
        return meeting;
    }

    public async Task<Meeting> UpdateMeeting(Meeting meeting)
    {
        context.Meetings.Update(meeting);
        await context.SaveChangesAsync();
        return meeting;
    }

    public async Task DeleteMeeting(int meetingId)
    {
        var meeting = await context.Meetings.FindAsync(meetingId);
        if (meeting != null)
        {
            context.Meetings.Remove(meeting);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Meeting?> GetMeetingById(int meetingId)
    {
        return await context.Meetings.FindAsync(meetingId);
    }

    public async Task<List<Meeting>> GetAllMeetings()
    {
        return await context.Meetings.ToListAsync();
    }
}
