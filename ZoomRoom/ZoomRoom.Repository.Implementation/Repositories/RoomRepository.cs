using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;


namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(SqliteDbContext sqliteDbContext) : base(sqliteDbContext)
        {
        }
    }
}
