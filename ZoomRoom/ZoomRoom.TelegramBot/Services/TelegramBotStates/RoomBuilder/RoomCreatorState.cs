using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;
using TelegramBot.Services;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace Telegrambot.Services.TelegramBotStates.RoomBuilder;


public class RoomCreatorState : State
{
    public RoomCreatorState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("До головного меню", "Спланувати зустріч");
        textMessage = "Введіть назву нової кімнати:";
    }

    public override async Task Initialize()
    {
        await CreateRoom();
    }

    private async Task CreateRoom()
    {
        //TODO: Create room;
        _telegramBotContext.roomData.Password = Guid.NewGuid().ToString().Substring(0, 8);

        await _telegramBotContext.roomService.CreateRoomAsync(_telegramBotContext.roomData);


        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната створена!");
        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId,
            $"Назва кімнати: {_telegramBotContext.roomData.Name}\n" +
            $"Пароль: {_telegramBotContext.roomData.Password}\n" +
            $"Ви можете використовувати ці дані для входу у кімнату"
        );

    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "До головного меню":
                    _telegramBotContext.state = new MainMenu(_telegramBotContext);
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
}
