using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IRoomService
{
    Task<Room> CreateRoomAsync(Room room);
    Task<Room> UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(int roomId);
    Task<Room?> GetRoomByIdAsync(int roomId);
    Task<List<Room>> GetAllRoomsAsync();
}
