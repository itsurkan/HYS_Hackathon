using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomBuilder;

public class RoomCreatorState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("До головного меню", "Спланувати зустріч");
        textMessage = "Введіть назву нової кімнати:";
        await CreateRoom();
    }

    private async Task CreateRoom()
    {
        //TODO: Create room;
        _telegramBotContext.roomData.Password = Guid.NewGuid().ToString().Substring(0, 8);

        await _telegramBotContext.roomService.CreateRoomAsync(_telegramBotContext.roomData);


        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната створена!");
        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId,
            $"Назва кімнати: {_telegramBotContext.roomData.Name}\n" +
            $"Пароль: {_telegramBotContext.roomData.Password}\n" +
            $"Ви можете використовувати ці дані для входу у кімнату"
        );
        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Що виконати далі?", replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        switch (answer)
        {
            case "До головного меню":
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Спланувати зустріч":
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            default:
                _telegramBotContext.state = this;
                break;
        }
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        return base.HandleCallbackQuery(callbackQuery);
    }
}
