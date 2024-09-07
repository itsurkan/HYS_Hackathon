using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IRoomService
{
    Task<Room> CreateRoomAsync(Room room);
    Task<Room> UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(long roomId);
    Task<Room?> GetRoomByIdAsync(long roomId);
    Task<List<Room>> GetAllRoomsAsync();
}
