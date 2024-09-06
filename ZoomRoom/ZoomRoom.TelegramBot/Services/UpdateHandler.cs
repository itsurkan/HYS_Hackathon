using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;

namespace Telegrambot.Services;

public class UpdateHandler : IUpdateHandler
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

    private void SentChatIdData(long chatId)
    {
        //TODO: Implement sending chat id data
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        

        if (update.Type is not UpdateType.Message) 
        {
            if(update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery!, cancellationToken);
            }
            return;
        } else 
        {


        long chatId = update.Message!.Chat.Id;

        if(update.Message.Text == "/start")
        {
            SentChatIdData(chatId);
        }

        if (!chatStates.ContainsKey(chatId))
        {
            chatStates[chatId] = new TelegramBotContext(botClient, chatId);
        }


        chatStates[chatId].state.HandleAnswer(update.Message?.Text);


        Message recievedMessage = await botClient.SendTextMessageAsync(chatId, 
                                    chatStates[chatId].state.textMessage, 
                                    replyMarkup: chatStates[chatId].state.keyboardMarkup);

                }

    }
}