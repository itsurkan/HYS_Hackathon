using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class AddUsersState : State
{
    public AddUsersState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
    }

    public async override Task Initialize()
    {
        textMessage = "Введіть логіни користувачів через кому:";
    
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public async override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
                    await _telegramBotContext.state.Initialize();
                    break;
                default:
                    await AddUsers(answer);
                    break;
            }
        }
        return;
    }

    private async Task AddUsers(string answer)
    {
        string[] users = answer.Split(',');
        foreach (string user in users)
        {
            ZoomRoom.Persistence.Models.User newUser = new ZoomRoom.Persistence.Models.User();
            newUser.Username = user;
            newUser.RoomUsers.Add(new RoomUser { Id = _telegramBotContext.roomData.Id, UserId = newUser.Id });

            await _telegramBotContext!.userService.CreateUserAsync(newUser);
        }
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувачі додані!");

        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
        await _telegramBotContext.state.Initialize();
    }
}
