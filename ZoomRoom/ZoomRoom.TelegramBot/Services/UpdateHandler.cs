using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZoomRoom.Services.PersistenceServices;
using ZoomRoom.TelegramBot.Services.TelegramBotStates;
using User = ZoomRoom.Persistence.Models.User;

namespace ZoomRoom.TelegramBot.Services;

public class UpdateHandler(IUserService userService, TelegramBotContext botContext, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    readonly Dictionary<long, TelegramBotContext> chatStates = [];

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery.From.Id;
        var state = chatStates[chatId].state;
        state.HandleCallbackQuery(callbackQuery);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("Message received");
        if (update.Type is not UpdateType.Message)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery!, cancellationToken);
            }

            return;
        }

        long chatId = update.Message!.Chat.Id;
        try
        {
            if (update.Message.Text == "/start")
            {
                await AddUserIfNotExists(update, chatId);
                botContext.Init(botClient, chatId);
                return;
            }

            if (!chatStates.TryGetValue(chatId, out TelegramBotContext? value))
            {
                value = botContext.Init(botClient, chatId);
                chatStates[chatId] = value;
            }

            var state = value.state;
            await state.HandleAnswer(update.Message.Text);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            logger.LogError("Error while handling update: {Message}", e.Message);
        }
    }

    private async Task AddUserIfNotExists(Update update, long chatId)
    {
        var user = await userService.GetUserByIdAsync(chatId);
        if (user is null)
        {
            user = new User
            {
                Id = chatId,
                Username = update.Message.Chat.Username ?? String.Empty,
                FirstName = update.Message.Chat.FirstName ?? string.Empty,
                LastName = update.Message.Chat.LastName ?? String.Empty
            };
            await userService.CreateUserAsync(user);
        }
    }
}
