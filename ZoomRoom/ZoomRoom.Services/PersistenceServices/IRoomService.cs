using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IRoomService
{
    Task<Room> CreateRoomAsync(Room room);
    Task<Room> UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(long roomId);
    Task<Room?> GetRoomByIdAsync(long roomId);
    Task<Room?> GetRoomByNameAsync(string roomName);
    Task<List<Room>> GetAllRoomsAsync();
    Task<List<Room>> GetAllRoomsWithUserAsync();
}
