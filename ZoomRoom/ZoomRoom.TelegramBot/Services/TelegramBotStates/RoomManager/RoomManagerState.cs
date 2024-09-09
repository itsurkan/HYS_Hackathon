using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

public class RoomManagerState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Назад");

        var inlineKeyboard = new InlineKeyboardMarkup();
        var rooms = await _telegramBotContext.roomService.GetAllRoomsAsync();

        if (!rooms.Any())
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(
                chatId: _telegramBotContext.chatId,
                text: "Немає кімнат"
            );
            _telegramBotContext.Init(_telegramBotContext.botClient, _telegramBotContext.chatId);
            return;
        }
        var inlineKeyboardButtons = rooms.Select(room =>
            InlineKeyboardButton.WithCallbackData(room.Name, room.Id.ToString(CultureInfo.InvariantCulture))
        ).ToArray();
         inlineKeyboard.AddButtons(inlineKeyboardButtons);

        await _telegramBotContext.botClient.SendTextMessageAsync(
            chatId: _telegramBotContext.chatId,
            text: "Оберіть кімнату:"
            , replyMarkup: inlineKeyboard
        );
    }

    public async override Task HandleAnswer(string answer)
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

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        _telegramBotContext.state = new RoomOptionsState(_telegramBotContext, long.Parse(callbackQuery.Data));
        await _telegramBotContext.state.Initialize();
    }
}
