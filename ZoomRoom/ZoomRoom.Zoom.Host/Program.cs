using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence;
using ZoomRoom.Services;
using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.Services;

namespace ZoomRoom.Zoom.Host;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<ZoomSettings>(context.Configuration.GetSection("ZoomSettings"));
                services.AddDbContext<SqliteDbContext>(options => options.UseSqlite("Data Source=../ZoomRoom.db"));
                services.AddHostedService<PollingService>();
                services.AddScoped<IZoomService, ZoomService>();
                services.AddPersistenceServices();
            })
            .Build();

        await host.RunAsync();
    }
}