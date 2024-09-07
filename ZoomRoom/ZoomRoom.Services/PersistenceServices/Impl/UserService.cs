using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Services.PersistenceServices.Impl;


public class UserService(IUserRepository userRepository) : IUserService

{
    public async Task<User> CreateUserAsync(User user)
    {
        userRepository.Create(user);
        await userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUserAsync(long userId)
    {
        var user = await userRepository.FindByIdAsync(userId);
        if (user != null)
        {
            userRepository.Delete(user);
            await userRepository.SaveChangesAsync();
        }
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        return await userRepository.FindByIdAsync(userId);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAll().ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string user) =>
        await userRepository.GetAll().Where(x=>x.Username ==user).FirstOrDefaultAsync();

    public async Task<bool> GetUserRoomAsync(long existingUserId, long roomDataId) =>
        await userRepository.GetAll().Where(x=>x.Id == existingUserId && x.RoomUsers.Any(r=>r.Id == roomDataId)).AnyAsync();
}
