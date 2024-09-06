using CMB.Persistence;
using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Services;

public class UserService(SqliteDbContext context)
{
    public async Task<User> CreateUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(long userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserById(long userId)
    {
        return await context.Users.FindAsync(userId);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await context.Users.ToListAsync();
    }
}
