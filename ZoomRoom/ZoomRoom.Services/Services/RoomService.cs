using Microsoft.EntityFrameworkCore;
using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    public async Task<Room> CreateRoomAsync(Room room)
    {
        room = _roomRepository.Create(room);
        await _roomRepository.SaveChangesAsync();

        return room;
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        _roomRepository.Update(room);
        await _roomRepository.SaveChangesAsync();

        return room;
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        var room = await _roomRepository.FindByIdAsync(roomId);
        if (room != null)
        {
            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync();
        }
    }

    public async Task<Room> GetRoomByIdAsync(int roomId)
    {
        return await _roomRepository.FindByIdAsync(roomId);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAll().ToListAsync();
    }
}
