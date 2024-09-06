using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingResultCheckState : State 
{
    public MeetingResultCheckState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).
            AddButton("Все вірно").
            AddNewRow().
            AddButtons("Змінити тему", "Змінити дату").
            AddButtons("Змінити тривалість", "Змінити пароль").
            AddButton("Змінити часовий пояс");

        _telegramBotContext.meetingData.IsFilled = true;

        textMessage = "Перевірте правильність введених даних:\n" +
            $"Тема: {_telegramBotContext.meetingData.MeetingName}\n" +
            $"Дата: {_telegramBotContext.meetingData.MeetingDate}\n" +
            $"Тривалість: {_telegramBotContext.meetingData.Duration}\n" +
            $"Пароль: {_telegramBotContext.meetingData.Passcode}\n" +
            $"Часовий пояс: {_telegramBotContext.meetingData.Timezone}\n" +
            "Все вірно?";
    }

    private void SentMeetingData()
    {
        int year, month, day, hour, minute;
        
        string[] data = _telegramBotContext.meetingData.MeetingDate.Split(['.',',',':',' ']);

        year = Int32.Parse(data[0], System.Globalization.NumberStyles.Number);
        month = Int32.Parse(data[1]);
        day = Int32.Parse(data[2]);
        hour = Int32.Parse(data[3]);
        minute = Int32.Parse(data[4]);

        int duration = 0;



        var meeting = new {
            dataTime = new DateTime(year, month, day, hour, minute, 0),
            meetingDuration = duration,
            passcode = _telegramBotContext.meetingData.Passcode,
        };
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        switch(answer)
        {
            case "Все вірно":

                SentMeetingData();

                await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Зустріч успішно створена!");

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
