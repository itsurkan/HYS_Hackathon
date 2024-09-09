using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class DeleteUserState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        textMessage = "Введіть логін користувача, якого бажаєте видалити:";
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
                    var users = await _telegramBotContext.userService.GetAllUsersAsync();
                    var user = users.FindLast(u => u.Username == answer);
                    if (user is not null)
                    {
                        await _telegramBotContext.userService.DeleteUserAsync(user.Id);
                        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувач видалений!");
                    }
                    else
                    {
                        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувача з таким логіном не існує!");
                    }
                    _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Id);
                    await _telegramBotContext.state.Initialize();
                    break;
        }
    }
}
