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

        _telegramBotContext.meetingData.isFilled = true;

        textMessage = "Перевірте правильність введених даних:\n" +
            $"Тема: {_telegramBotContext.meetingData.meetingName}\n" +
            $"Дата: {_telegramBotContext.meetingData.meetingDate}\n" +
            $"Тривалість: {_telegramBotContext.meetingData.duration}\n" +
            $"Пароль: {_telegramBotContext.meetingData.passcode}\n" +
            $"Часовий пояс: {_telegramBotContext.meetingData.timezone}\n" +
            "Все вірно?";
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        switch(answer)
        {
            case "Все вірно":

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
