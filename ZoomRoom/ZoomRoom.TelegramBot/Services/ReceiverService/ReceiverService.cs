using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegrambot.Services.ReceiverService;

namespace ZoomRoom.TelegramBot.Services.ReceiverService;

public class ReceiverService(
    ITelegramBotClient telegramBotClient,
    IUpdateHandler updateHandler)
    : IReceiverService
{
    public async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = [],
            DropPendingUpdates = true,
        };

        var me = await telegramBotClient.GetMeAsync(cancellationToken);

        await telegramBotClient.ReceiveAsync(
            updateHandler: updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken);
    }

}
