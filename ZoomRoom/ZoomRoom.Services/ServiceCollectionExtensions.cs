using Microsoft.Extensions.DependencyInjection;
using ZoomRoom.Repository.Contracts.IRepositories;
using ZoomRoom.Repository.Implementation.Repositories;
using ZoomRoom.Services.PersistenceServices;
using ZoomRoom.Services.PersistenceServices.Impl;

namespace ZoomRoom.Services;

public static class ServiceCollectionExtensions // call in registration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddDbContext<SqliteDbContext>(options => options.UseSqlite("Data Source=ZoomRoom.db"));
        return services;
    }
}
