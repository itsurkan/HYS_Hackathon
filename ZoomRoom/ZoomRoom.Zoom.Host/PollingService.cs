using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.PersistenceServices;

namespace ZoomRoom.Zoom.Host;

public class PollingService(
    IServiceProvider serviceProvider,
    ILogger<PollingService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();

                await CheckEventsToStart(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                logger.LogError("Polling failed with exception: {Exception}", ex);

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
            await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
        }
    }

    private async Task CheckEventsToStart(IServiceProvider sp)
    {
        var meetingService = sp.GetRequiredService<IMeetingService>();
        var zoomService = sp.GetRequiredService<IZoomService>();

        var meetings = await meetingService.GetMeetingsToStartAsync(DateTime.UtcNow);

        foreach (var meeting in meetings)
        {
           var token =  await zoomService.GetAccessTokenAsync();
           var response = await zoomService.StartMeetingAsync(token, meeting.ZoomMeetingId);
        }
    }
}
