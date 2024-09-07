using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using TelegramBot.Services;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class RoomOptionsState : State
{
    private readonly string roomName;

    public RoomOptionsState(TelegramBotContext telegramBotContext, string roomName) :
        base(telegramBotContext)
    {
        this.roomName = roomName;

    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Додати користувачів", "Видалити користувачів").AddNewRow().AddButton("Видалити кімнату");
        textMessage = $"Що бажаєте зробиит з кімнатою {roomName}?";
        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Додати користувачів":
                    _telegramBotContext.roomData = await _telegramBotContext.roomService.GetRoomByNameAsync(roomName);
                    _telegramBotContext.state = new AddUsersState(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                    break;
                case "Видалити користувачів":
                    _telegramBotContext.roomData = await _telegramBotContext.roomService.GetRoomByNameAsync(roomName);
                    _telegramBotContext.state = new DeleteUserState(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                    break;
                case "Видалити кімнату":
                    await RoomDeletion();
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }
        }
    }

    private async Task RoomDeletion()
    {
        await _telegramBotContext!.roomService.DeleteRoomAsync(_telegramBotContext.roomData.Id);
        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната видалена!");
        _telegramBotContext.state = new MainMenu(_telegramBotContext);
        await _telegramBotContext.state.Initialize();
    }
}
