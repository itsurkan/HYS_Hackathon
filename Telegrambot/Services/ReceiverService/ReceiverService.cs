using System;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Telegrambot.Services.ReceiverService;

public class ReceiverService : IReceiverService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUpdateHandler _updateHandler;

    public ReceiverService(ITelegramBotClient telegramBotClient, 
                                IUpdateHandler updateHandler)
    {
        _telegramBotClient = telegramBotClient;
        _updateHandler = updateHandler;
    }

    public async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = [],
            DropPendingUpdates = true,
        };

        var me = await _telegramBotClient.GetMeAsync(cancellationToken);

        await _telegramBotClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken);
    }

}
