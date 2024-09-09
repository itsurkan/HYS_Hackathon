using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;

public class MeetingDeleteState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public async override Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");

        InlineKeyboardMarkup button = new InlineKeyboardMarkup();

        List<Meeting> meetings = await _telegramBotContext.meetingService.GetAllMeetingsAsync();

        if (!meetings.Any())
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(
                chatId: _telegramBotContext.chatId,
                text: "Немає мітингів"
            );
            _telegramBotContext.Init(_telegramBotContext.botClient, _telegramBotContext.chatId);
            return;
        }

        List<Meeting> finalMeetings = meetings
            .ToList();

        foreach (Meeting meeting in finalMeetings)
        {

            button.AddButtons($"{meeting.Title}");
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Оберіть зустріч для видалення:",  replyMarkup: button);
        }
    }

    public async override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new MainMenu(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }
        }
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        await _telegramBotContext.meetingService.DeleteMeetingAsync(callbackQuery.Data);
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Мітинг видалено");

    }
}
