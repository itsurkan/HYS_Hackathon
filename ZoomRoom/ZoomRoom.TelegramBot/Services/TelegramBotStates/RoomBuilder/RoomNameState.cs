using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;

namespace Telegrambot.Services.TelegramBotStates;

public class RoomNameState : State
{
    public RoomNameState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");

        textMessage = "Введіть назву нової кімнати:";
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if(answer == "Назад")
        {
            _telegramBotContext.state = new MainMenu(_telegramBotContext);  
        }

        if(answer is null || answer == "")
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Назва кімнати не може бути пустою ");
            _telegramBotContext.state = new RoomCreatorState(_telegramBotContext, answer);
        }
        else
        {
            _telegramBotContext.state = new RoomCreatorState(_telegramBotContext, answer);
        }
    }
}
