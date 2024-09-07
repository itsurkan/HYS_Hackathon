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


    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");
        await _telegramBotContext.state.Initialize();

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

    public async override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new MainMenu(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }

        }
    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, callbackQuery.Data);
        await _telegramBotContext.state.Initialize();
    }

}
