using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class MeetingRepository : RepositoryBase<Meeting>, IMeetingRepository
    {
        public MeetingRepository(SqliteDbContext sqliteDbContext) : base(sqliteDbContext)
        {
        }
    }
}
