using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Services.PersistenceServices.Impl;


public class MeetingService(IMeetingRepository meetingRepository) : IMeetingService
{
    public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
    {
        meetingRepository.Create(meeting);
        await meetingRepository.SaveChangesAsync();

        return meeting;
    }

    public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
    {
        meetingRepository.Update(meeting);
        await meetingRepository.SaveChangesAsync();

        return meeting;
    }

    public async Task DeleteMeetingAsync(long meetingId)
    {
        var meeting = await meetingRepository.FindByIdAsync(meetingId);
        if (meeting != null)
        {
            meetingRepository.Delete(meeting);
            await meetingRepository.SaveChangesAsync();
        }
    }

    public async Task<Meeting?> GetMeetingByIdAsync(long meetingId)
    {
        return await meetingRepository.FindByIdAsync(meetingId);
    }

    public async Task<List<Meeting>> GetAllMeetingsAsync()
    {
        return await meetingRepository.GetAll().ToListAsync();
    }
}
