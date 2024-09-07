using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegrambot.Services.ReceiverService;

namespace ZoomRoom.TelegramBot.Services;

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
                var receiverService = scope.ServiceProvider.GetRequiredService<IReceiverService>();

                await receiverService.ReceiveAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
