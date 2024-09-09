using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;
using User = ZoomRoom.Persistence.Models.User;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class AddUsersState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public async override Task Initialize()
    {
        textMessage = "Введіть логіни користувачів через кому:";

        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public async override Task HandleAnswer(string answer)
    {
        switch (answer)
        {
            case "Назад":
                _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Id);
                await _telegramBotContext.state.Initialize();
                break;
            default:
                await AddUsers(answer);
                break;
        }
    }

    private async Task AddUsers(string answer)
    {
        string[] users = answer.Split(',');
        foreach (string user in users)
        {
            var newUser = new User
            {
                Username = user
            };
            var existingUser = await _telegramBotContext.userService.GetUserByUsernameAsync(user) ?? await _telegramBotContext.userService.CreateUserAsync(newUser);

            var userRoom = await _telegramBotContext.userService.GetUserRoomAsync(existingUser!.Id, _telegramBotContext.roomData.Id);
            if (!userRoom)
            {
                var newRoomUser = new RoomUser { Id = _telegramBotContext.roomData.Id, UserId = existingUser.Id };
                await _telegramBotContext.roomService.CreateRoomUserAsync(newRoomUser);
            }
        }

        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувачі додані!");

        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Id);
        await _telegramBotContext.state.Initialize();
    }
}
