using System;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;

namespace StudyBot.Services;

public class MainMenu : State
{
    public MainMenu(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        textMessage = "Вітаємо в BOTNAME!?";
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Створити кімнату", "Спланувати зустріч");
    
    }

    public override void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(_telegramBotContext));
        }

        switch (answer)
        {
            case "Створити кімнату":
                _telegramBotContext.state = new RoomNameState(_telegramBotContext);
                break;
            case "Спланувати зустріч":
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                break;
            default:
                _telegramBotContext.state = this;
                break;
        }
    }
}
