using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;
using TelegramBot.Services;

namespace Telegrambot.Services.TelegramBotStates;

public class RoomNameState : State
{
    public RoomNameState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {

    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");

        textMessage = "Введіть назву нової кімнати:";

        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Назва кімнати не може бути пустою ");
                _telegramBotContext.state = new RoomCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }
            else if (answer == "Назад")
            {
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }
            else
            {
                _telegramBotContext.roomData.Name = answer;
                _telegramBotContext.state = new RoomCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }


        }
    }
}
