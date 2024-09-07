using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class MeetingRepository(SqliteDbContext dbContext) : RepositoryBase<Meeting>(dbContext), IMeetingRepository;
}
