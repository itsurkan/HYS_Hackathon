using Microsoft.EntityFrameworkCore;
using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.Services;

public class MeetingService
{
    private readonly IMeetingRepository _meetingRepository;

    public MeetingService(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }
    public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
    {
        _meetingRepository.Create(meeting);
        await _meetingRepository.SaveChangesAsync();

        return meeting;
    }

    public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
    {
        _meetingRepository.Update(meeting);
        await _meetingRepository.SaveChangesAsync();

        return meeting;
    }

    public async Task DeleteMeetingAsync(long meetingId)
    {
        var meeting = await _meetingRepository.FindByIdAsync(meetingId);
        if (meeting != null)
        {
            _meetingRepository.Delete(meeting);
            await _meetingRepository.SaveChangesAsync();
        }
    }

    public async Task<Meeting?> GetMeetingByIdAsync(long meetingId)
    {
        return await _meetingRepository.FindByIdAsync(meetingId);
    }

    public async Task<List<Meeting>> GetAllMeetingsAsync()
    {
        return await _meetingRepository.GetAll().ToListAsync();
    }
}
