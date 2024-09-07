using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Services.PersistenceServices.Impl;


public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    public async Task<Room> CreateRoomAsync(Room room)
    {
        room = roomRepository.Create(room);
        await roomRepository.SaveChangesAsync();

        return room;
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        roomRepository.Update(room);
        await roomRepository.SaveChangesAsync();

        return room;
    }

    public async Task DeleteRoomAsync(long roomId)
    {
        var room = await roomRepository.FindByIdAsync(roomId);
        if (room != null)
        {
            roomRepository.Delete(room);
            await roomRepository.SaveChangesAsync();
        }
    }

    public async Task<Room> GetRoomByIdAsync(long roomId)
    {
        return await roomRepository.FindByIdAsync(roomId);
    }

    public async Task<Room?> GetRoomByNameAsync(string roomName)
    {
        return await roomRepository.GetAll().FirstOrDefaultAsync(x=>x.Name == roomName);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await roomRepository.GetAll().ToListAsync();
    }

    public async Task<List<Room>> GetAllRoomsWithUserAsync()
    {
        return await roomRepository.GetAllRoomsWithUsersAsync();
    }
}
