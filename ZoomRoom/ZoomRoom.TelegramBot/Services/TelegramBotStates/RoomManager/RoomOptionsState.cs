using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using TelegramBot.Services;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class RoomOptionsState : State
{
    private string roomName; 

    public RoomOptionsState(TelegramBotContext telegramBotContext, string roomName) :
        base(telegramBotContext)
    {
        this.roomName = roomName;

        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Додати користувачів", "Видалити користувачів").
            AddNewRow().AddButton("Видалити кімнату");
        textMessage = $"Що бажаєте зробиит з кімнатою {roomName}?";
    }

    public override void HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Додати користувачів":
                    _telegramBotContext.roomData = _telegramBotContext.roomService.GetAllRoomsAsync().Result.FirstOrDefault(r => r.Name == roomName);
                    _telegramBotContext.state = new AddUsersState(_telegramBotContext);
                    break;
                case "Видалити користувачів":
                    _telegramBotContext.roomData = _telegramBotContext.roomService.GetAllRoomsAsync().Result.FirstOrDefault(r => r.Name == roomName);
                    _telegramBotContext.state = new DeleteUserState(_telegramBotContext);
                    break;
                case "Видалити кімнату":
                    RoomDeletion();
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }

        }
    }

    private async void RoomDeletion()
    {
        await _telegramBotContext!.roomService.DeleteRoomAsync(_telegramBotContext.roomData.Id);
        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната видалена!");
        _telegramBotContext.state = new MainMenu(_telegramBotContext);
    }
}