using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class AddUsersState : State
{
    public AddUsersState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        textMessage = "Введіть логіни користувачів через кому:";

        keyboardMarkup =  new ReplyKeyboardMarkup(true).AddButtons("Назад");
    }

    public override void HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
                    break;
                default:
                    AddUsers(answer);
                    break;
            }
        }
    }

    private async void AddUsers(string answer)
    {
        string[] users = answer.Split(',');
        foreach (string user in users)
        {
            ZoomRoom.Persistence.Models.User newUser = new ZoomRoom.Persistence.Models.User();
            newUser.Username = user;
            newUser.RoomUsers.Add(new RoomUser { Id = _telegramBotContext.roomData.Id, UserId = newUser.Id });

            await _telegramBotContext!.userService.CreateUserAsync(newUser);
        }
        _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Користувачі додані!");

        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, _telegramBotContext.roomData.Name);
    }   
}
