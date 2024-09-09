using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;

public class MeetingManagerState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public async override Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Оберіть зустріч для редагування:");

        InlineKeyboardMarkup button = new InlineKeyboardMarkup();

        List<Task<Room>> roomTasks = (await _telegramBotContext.userService.GetUserByIdAsync(_telegramBotContext.chatId))
            .RoomUsers
            .Select(async ru => await _telegramBotContext.roomService.GetRoomByIdAsync(ru.Id))
            .Cast<Task<Room>>()
            .ToList();

        List<Room> rooms = (await Task.WhenAll(roomTasks)).ToList();

        List<Meeting> meetings = await _telegramBotContext.meetingService.GetAllMeetingsAsync();

        List<Meeting> finalMeetings = meetings
            // .Where(m => rooms.Any(r => m.RoomId == r.Id))
            .ToList();

        foreach (Meeting meeting in finalMeetings)
        {
            string meetingStatus = DateTime.Compare(DateTime.Now, meeting.ScheduledTime.AddMinutes(meeting.Duration)) > 0 ? "Завершено" : "Не завершено";

            button.AddButtons($"{meeting.Title}, {meetingStatus}");
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Доступні мітинги", replyMarkup: button);
        }
    }

    public async override Task HandleAnswer(string answer)
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

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        return Task.CompletedTask;
    }
}
