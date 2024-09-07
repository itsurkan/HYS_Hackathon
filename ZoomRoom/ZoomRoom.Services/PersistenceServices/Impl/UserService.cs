using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Repository.Contracts.IRepositories;

namespace ZoomRoom.Services.PersistenceServices.Impl;


public class UserService : IUserService

{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<User> CreateUserAsync(User user)
    {
        _userRepository.Create(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUserAsync(long userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user != null)
        {
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        return await _userRepository.FindByIdAsync(userId);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAll().ToListAsync();
    }
}
