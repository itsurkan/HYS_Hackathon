using Microsoft.Extensions.DependencyInjection;
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
        // services.AddDbContext<SqliteDbContext>(options => options.UseSqlite("Data Source=ZoomRoom.db"));
        return services;
    }
}
