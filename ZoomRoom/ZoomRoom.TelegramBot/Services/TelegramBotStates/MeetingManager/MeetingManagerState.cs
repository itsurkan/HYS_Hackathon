using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using TelegramBot.Services;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;

public class MeetingManagerState : State
{
    public MeetingManagerState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Оберіть зустріч для редагування:");

        InlineKeyboardMarkup button = new InlineKeyboardMarkup();

        List<Room> rooms = telegramBotContext.userService
            .GetUserByIdAsync(_telegramBotContext.chatId)
            .Result
            .RoomUsers
            .Select(ru => _telegramBotContext.roomService.GetRoomByIdAsync(ru.RoomId).Result)
            .ToList();

        List<Meeting> meetings = telegramBotContext.meetingService.GetAllMeetingsAsync().Result;

        List<Meeting> finalMeetings = meetings
            .Where(m => rooms.Any(r => m.RoomId == r.Id))
            .ToList();

        foreach (Meeting meeting in finalMeetings)
        {
            string meetingStatus = DateTime.Compare(DateTime.Now, meeting.ScheduledTime.AddMinutes(meeting.Duration)) > 0 ? "Завершено" : "Не завершено"; 

            button.AddButtons(InlineKeyboardButton.WithCallbackData(
                $"meeting.Title \n" +
                $"meeting.ScheduledTime \n" +
                $" {meetingStatus}", meeting.Id.ToString()));
        }
    }

    public override void HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new MainMenu(_telegramBotContext);
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }
        }
    }

    public override void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        _telegramBotContext.state = new MeetingInteractiveState(_telegramBotContext, int.Parse(callbackQuery.Data));

        
    }
}
