using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;


namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class RoomRepository(SqliteDbContext dbContext) : RepositoryBase<Room>(dbContext), IRoomRepository
    {
        public async Task<List<Room>> GetAllRoomsWithUsersAsync() =>
            await dbContext.Set<Room>().Include(x => x.RoomUsers).Where(x => x.RoomUsers.Count != 0).ToListAsync();

        public RoomUser AddRoomUser(RoomUser roomUser)
        {
            dbContext.Set<RoomUser>().Add(roomUser);
            return roomUser;
        }
    }
}
