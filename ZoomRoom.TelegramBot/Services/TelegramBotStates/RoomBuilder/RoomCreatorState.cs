using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;

namespace Telegrambot.Services.TelegramBotStates.RoomBuilder;

public class RoomData 
{
    public string name;
    public string password;
    public bool isFilled = false;
}

public class RoomCreatorState : State
{
    public RoomCreatorState(TelegramBotContext telegramBotContext, string roomName) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("До головного меню", "Спланувати зустріч");
        textMessage = "Введіть назву нової кімнати:";


        CreateRoom(roomName);
    }

    private void CreateRoom(string roomName)
    {
        //TODO: Create room
        _telegramBotContext.roomData.name = roomName;
        _telegramBotContext.roomData.password = Guid.NewGuid().ToString().Substring(0, 8);

        _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната створена!");
        _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, 
            $"Назва кімнати: {_telegramBotContext.roomData.name}\n" +
            $"Пароль: {_telegramBotContext.roomData.password}\n" +
            $"Ви можете використовувати ці дані для входу у кімнату"
        );

    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

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
