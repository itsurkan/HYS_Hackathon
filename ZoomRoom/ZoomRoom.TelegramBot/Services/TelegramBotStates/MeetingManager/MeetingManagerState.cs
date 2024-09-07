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


    }

    public async override Task Initialize()
    {
        InlineKeyboardMarkup button = new InlineKeyboardMarkup();

        List<Task<Room>> roomTasks = (await _telegramBotContext.userService.GetUserByIdAsync(_telegramBotContext.chatId))            
            .RoomUsers
            .Select(async ru => await _telegramBotContext.roomService.GetRoomByIdAsync(ru.Id))
            .ToList();
        
        List<Room> rooms = (await Task.WhenAll(roomTasks)).ToList();

        List<Meeting> meetings = await _telegramBotContext.meetingService.GetAllMeetingsAsync();

        List<Meeting> finalMeetings = meetings
            .Where(m => rooms.Any(r => m.RoomId == r.Id))
            .ToList();

        foreach (Meeting meeting in finalMeetings)
        {
            string meetingStatus = DateTime.Compare(DateTime.Now, meeting.ScheduledTime.AddMinutes(meeting.Duration)) > 0 ? "Завершено" : "Не завершено";

            button.AddButtons(
                $"meeting.Title \n" +
                $"meeting.ScheduledTime \n" +
                $" {meetingStatus}"
                );
        }    }

    public override Task HandleAnswer(string answer)
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
        return Task.CompletedTask;
    }

    public override void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
    }
}
