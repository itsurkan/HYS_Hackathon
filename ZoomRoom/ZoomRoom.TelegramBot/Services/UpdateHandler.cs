using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegrambot.Services.TelegramBotStates;
using ZoomRoom.Services.PersistenceServices;
using User = ZoomRoom.Persistence.Models.User;

namespace Telegrambot.Services;

public class UpdateHandler(IMeetingService meetingService, IRoomService roomService, IUserService userService, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    readonly Dictionary<long, TelegramBotContext> chatStates = new ();

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        long chatId = callbackQuery.From.Id;

        chatStates[chatId].state.HandleCallbackQuery(callbackQuery);
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

            if (!chatStates.ContainsKey(chatId))
            {
                chatStates[chatId] = new TelegramBotContext(
                    botClient,
                    chatId,
                    userService,
                    roomService,
                    meetingService
                );
            }


            chatStates[chatId].state.HandleAnswer(update.Message?.Text);


            // Message recievedMessage = await botClient.SendTextMessageAsync(chatId,
            //                             chatStates[chatId].state.textMessage,
            //                             replyMarkup: chatStates[chatId].state.keyboardMarkup);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
