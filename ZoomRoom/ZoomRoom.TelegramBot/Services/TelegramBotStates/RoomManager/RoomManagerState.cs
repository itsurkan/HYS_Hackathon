using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using TelegramBot.Services;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class RoomManagerState : State
{
    public RoomManagerState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");


    }

    public override async Task Initialize()
    {
        await _telegramBotContext.botClient.SendTextMessageAsync(
            chatId: _telegramBotContext.chatId,
            text: "Оберіть кімнату:"
        );

        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
        List<Room> rooms = _telegramBotContext.roomService.GetAllRoomsAsync().Result.SelectMany(u => u.RoomUsers)
                    .Select(ru => ru.Room)
                    .ToList();
        foreach (Room room in rooms) inlineKeyboard.AddButtons(room.Name);
    }

    public override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new MainMenu(_telegramBotContext);
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }

        }
        return Task.CompletedTask;

    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, callbackQuery.Data);
    }

}
