using CMB.Persistence;
using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services;

public class RoomService(SqliteDbContext context)
{
    public async Task<Room> CreateRoom(Room room)
    {
        context.Rooms.Add(room);
        await context.SaveChangesAsync();
        return room;
    }

    public async Task<Room> UpdateRoom(Room room)
    {
        context.Rooms.Update(room);
        await context.SaveChangesAsync();
        return room;
    }

    public async Task DeleteRoom(int roomId)
    {
        var room = await context.Rooms.FindAsync(roomId);
        if (room != null)
        {
            context.Rooms.Remove(room);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Room> GetRoomById(int roomId)
    {
        return await context.Rooms.FindAsync(roomId);
    }

    public async Task<List<Room>> GetAllRooms()
    {
        return await context.Rooms.ToListAsync();
    }
}
