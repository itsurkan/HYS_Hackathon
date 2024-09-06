using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices.Impl;

public class RoomService(SqliteDbContext context)
{
    public async Task<Room> CreateRoomAsync(Room room)
    {
        context.Rooms.Add(room);
        await context.SaveChangesAsync();
        return room;
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        context.Rooms.Update(room);
        await context.SaveChangesAsync();
        return room;
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        var room = await context.Rooms.FindAsync(roomId);
        if (room != null)
        {
            context.Rooms.Remove(room);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Room> GetRoomByIdAsync(int roomId)
    {
        return await context.Rooms.FindAsync(roomId);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await context.Rooms.ToListAsync();
    }
}
