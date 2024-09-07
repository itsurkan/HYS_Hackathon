using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Domain.Requests;
using ZoomRoom.Domain.Responses;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingResultCheckState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Все вірно").AddNewRow().AddButtons("Змінити тему", "Змінити дату").AddNewRow()
            .AddButtons("Змінити тривалість", "Змінити пароль").AddNewRow().AddButton("Змінити часовий пояс");

        _telegramBotContext!.MeetingFormIsFilled = true;

        var room = await _telegramBotContext.roomService.GetRoomByIdAsync(_telegramBotContext!.meetingData.RoomId);

        textMessage = "Перевірте правильність введених даних:\n" +
                      $"Тема: {_telegramBotContext.meetingData.Title}\n" +
                      $"Дата: {_telegramBotContext.meetingData.ScheduledTime.ToString(CultureInfo.InvariantCulture)}\n" +
                      $"Тривалість: {_telegramBotContext.meetingData.Duration}\n" +
                      $"Пароль: {_telegramBotContext.meetingData.Password}\n" +
                      $"Часовий пояс: {_telegramBotContext.meetingData.TimeZone}\n" +
                      $"Кімната: {room?.Name}\n" +
                      "Все вірно?";

        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        switch (answer)
        {
            case "Все вірно":
                await GreateMeeting();
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                break;
            case "Змінити тему":
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                break;
            case "Змінити дату":
                _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
                break;
            case "Змінити тривалість":
                _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
                break;
            case "Змінити пароль":
                _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
                break;
            case "Змінити часовий пояс":
                _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
                break;
        }
        await _telegramBotContext.state.Initialize();

    }

    private async Task GreateMeeting()
    {
        _telegramBotContext.MeetingFormIsFilled = false;
        var meeting = await GetZoomLink();
        _telegramBotContext.meetingData.ZoomLink = meeting!.JoinUrl;
        _telegramBotContext.meetingData.ZoomMeetingId = long.Parse(meeting.Id);
        await _telegramBotContext.meetingService.CreateMeetingAsync(_telegramBotContext.meetingData);
        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Зустріч успішно створена! Посилання на зустріч: " + meeting.JoinUrl);
    }

    private async Task<MeetingResponse?> GetZoomLink()
    {
        var token = await _telegramBotContext!.zoomService.GetAccessTokenAsync();
        var meeting = await _telegramBotContext.zoomService.CreateMeetingAsync(token, new MeetingBodyRequest(
            _telegramBotContext.meetingData.Title,
            _telegramBotContext.meetingData.ScheduledTime,
            _telegramBotContext.meetingData.Duration,
            "Agenga",
            _telegramBotContext.meetingData.TimeZone));
        return meeting;
    }
}
