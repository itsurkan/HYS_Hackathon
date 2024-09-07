using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegrambot.Services.TelegramBotStates;
using ZoomRoom.Persistence;
using ZoomRoom.Services.PersistenceServices;

namespace Telegrambot.Services;

public class UpdateHandler() : IUpdateHandler
{
    Dictionary<long, TelegramBotContext> chatStates = new Dictionary<long, TelegramBotContext>();

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
        if (update.Type is not UpdateType.Message)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery!, cancellationToken);
            }
            return;
        }

        long chatId = update.Message!.Chat.Id;

        if (update.Message.Text == "/start")
        {
            using (var db = new SqliteDbContext(new DbContextOptions<SqliteDbContext>()))
            {
                ZoomRoom.Persistence.Models.User user = db.Users.FirstOrDefault(u => u.Id == chatId);
                if (user is null)
                {
                    user = new ZoomRoom.Persistence.Models.User
                    {
                        Id = chatId,
                        Username = update.Message.Chat.Username ?? String.Empty,
                        FirstName = update.Message.Chat.FirstName,
                        LastName = update.Message.Chat.LastName ?? String.Empty
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }

        if (!chatStates.ContainsKey(chatId))
        {
            // chatStates[chatId] = new TelegramBotContext(botClient, chatId,
            //      userService, roomService,meetingService
            // );
        }


        chatStates[chatId].state.HandleAnswer(update.Message?.Text);


        // Message recievedMessage = await botClient.SendTextMessageAsync(chatId,
        //                             chatStates[chatId].state.textMessage,
        //                             replyMarkup: chatStates[chatId].state.keyboardMarkup);
    }
}
