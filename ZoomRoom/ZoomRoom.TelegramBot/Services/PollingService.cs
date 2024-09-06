using System;
using Microsoft.Extensions.Hosting;
using Telegrambot.Services.ReceiverService;

namespace Telegrambot.Services;

public class PollingService : BackgroundService
{
    private IReceiverService _receiverService;

    public PollingService(IReceiverService receiverService)
    {
        _receiverService = receiverService;
    }

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
                await _receiverService.ReceiveAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }

}
