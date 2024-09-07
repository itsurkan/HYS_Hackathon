using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Repository.Contracts.IRepositories
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        Task<List<Room>> GetAllRoomsWithUsersAsync();
    }
}
