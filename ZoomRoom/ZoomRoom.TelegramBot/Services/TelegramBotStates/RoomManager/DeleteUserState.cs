using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class DeleteUserState : State
{

    public DeleteUserState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");

        textMessage = "Введіть логін користувача, якого бажаєте видалити:";
    }

    public override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
                    break;
                default:
                    DeleteUser(answer);
                    break;
            }
        }
        return Task.CompletedTask;
    }

    private async void DeleteUser(string answer)
    {
        List<Persistence.Models.User> users = await _telegramBotContext.userService.GetAllUsersAsync();
        Persistence.Models.User user = users.FindLast(u => u.Username == answer);
        if (user is not null)
        {
            await _telegramBotContext.userService.DeleteUserAsync(user.Id);
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувач видалений!");
        }
        else
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувача з таким логіном не існує!");
        }
        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
    }
}
