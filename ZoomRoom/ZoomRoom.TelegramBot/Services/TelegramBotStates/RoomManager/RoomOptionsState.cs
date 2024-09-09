using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class RoomOptionsState(TelegramBotContext telegramBotContext, long roomId) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Додати користувачів", "Видалити користувачів").AddNewRow().AddButtons("Видалити кімнату", "Назад");
        _telegramBotContext.roomData.Id = roomId;
        var room = await _telegramBotContext.roomService.GetRoomByIdAsync(roomId);
            textMessage = $"Що бажаєте зробиит з кімнатою {room.Name}?";

        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        switch (answer)
        {
            case "Додати користувачів":
                _telegramBotContext.roomData = await _telegramBotContext.roomService.GetRoomByIdAsync(roomId);
                _telegramBotContext.state = new AddUsersState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Видалити користувачів":
                _telegramBotContext.roomData = await _telegramBotContext.roomService.GetRoomByIdAsync(roomId);
                _telegramBotContext.state = new DeleteUserState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Видалити кімнату":
                await RoomDeletion();
                break;
            case "Назад":
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            default:
                _telegramBotContext.state = this;
                break;
        }
    }

    private async Task RoomDeletion()
    {
        await _telegramBotContext.roomService.DeleteRoomAsync(_telegramBotContext.roomData.Id);
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната видалена!");
        _telegramBotContext.state = new MainMenu(_telegramBotContext);
        await _telegramBotContext.state.Initialize();
    }
}
