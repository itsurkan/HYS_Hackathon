using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SqliteDbContext dbContext) : base(dbContext)
        {
        }
    }
}
