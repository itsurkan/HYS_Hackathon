using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;
using ZoomRoom.Domain.Requests;
using ZoomRoom.Persistence.Models;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingResultCheckState : State
{
    public MeetingResultCheckState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Все вірно").AddNewRow().AddButtons("Змінити тему", "Змінити дату").AddNewRow()
            .AddButtons("Змінити тривалість", "Змінити пароль").AddNewRow().AddButton("Змінити часовий пояс");

        _telegramBotContext!.MeetingFormIsFilled = true;

        Room room = _telegramBotContext!.roomService!.GetRoomByIdAsync(_telegramBotContext!.meetingData.RoomId).Result;

        textMessage = "Перевірте правильність введених даних:\n" +
                      $"Тема: {_telegramBotContext.meetingData.Title}\n" +
                      $"Дата: {_telegramBotContext.meetingData.ScheduledTime.ToString()}\n" +
                      $"Тривалість: {_telegramBotContext.meetingData.Duration}\n" +
                      $"Пароль: {_telegramBotContext.meetingData.Password}\n" +
                      $"Часовий пояс: {_telegramBotContext.meetingData.TimeZone}\n" +
                      $"Кімната: {room.Name}\n" +
                      "Все вірно?";

        _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }


    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Все вірно":
                    await _telegramBotContext.meetingService.CreateMeetingAsync(_telegramBotContext.meetingData);
                    _telegramBotContext.MeetingFormIsFilled = false;
                    var meetingLink = await GetZoomLink();
                    await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Зустріч успішно створена! Посилання на зустріч: " + meetingLink);

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
        }
    }

    private async Task<string> GetZoomLink()
    {
        var token = await _telegramBotContext!.zoomService.GetAccessTokenAsync();
        var meetingLink = await _telegramBotContext.zoomService.CreateMeetingAsync(token, new MeetingBodyRequest(
            _telegramBotContext.meetingData.Title,
            _telegramBotContext.meetingData.ScheduledTime,
            _telegramBotContext.meetingData.Duration,
            "Agenga",
            _telegramBotContext.meetingData.TimeZone));
        return meetingLink;
    }
}
