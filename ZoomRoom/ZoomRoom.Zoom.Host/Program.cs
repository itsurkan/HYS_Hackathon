using ZoomRoom.Services;
using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.Services;
using ZoomRoom.Zoom.Host;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<ZoomSettings>(context.Configuration.GetSection("ZoomSettings"));
        services.AddHostedService<PollingService>();
        services.AddScoped<IZoomService, ZoomService>();
        services.AddPersistenceServices();
    })
    .Build();

await host.RunAsync();
