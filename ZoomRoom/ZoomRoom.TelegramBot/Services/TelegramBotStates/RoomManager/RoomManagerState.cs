using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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


        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
        List<Room> rooms = (await _telegramBotContext.roomService.GetAllRoomsAsync()).SelectMany(u => u.RoomUsers)
                    .Select(ru => ru.Room)
                    .ToList();

        foreach (Room room in rooms) inlineKeyboard.AddButtons(room.Name);

        await _telegramBotContext.botClient.SendTextMessageAsync(
            chatId: _telegramBotContext.chatId,
            text: "Оберіть кімнату:"
            , replyMarkup: inlineKeyboard
        );
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
