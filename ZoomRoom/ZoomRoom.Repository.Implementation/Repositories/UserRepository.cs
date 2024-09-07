using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Repository.Implementation.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SqliteDbContext sqliteDbContext) : base(sqliteDbContext)
        {
        }
    }
}
