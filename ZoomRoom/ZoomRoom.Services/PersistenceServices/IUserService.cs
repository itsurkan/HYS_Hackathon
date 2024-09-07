using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services.PersistenceServices;

public interface IUserService
{
    Task<User?> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(long userId);
    Task<User?> GetUserByIdAsync(long userId);
    Task<List<User>> GetAllUsersAsync();
}
