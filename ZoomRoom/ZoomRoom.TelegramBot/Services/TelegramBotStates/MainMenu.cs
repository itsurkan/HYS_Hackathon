using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomBuilder;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates;

public class MainMenu : State
{
    public MainMenu(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {

        textMessage = "Вітаємо в ZoomRoom! Що бажаєте зробити?";
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Створити кімнату", "Спланувати зустріч").
            AddNewRow().AddButtons("Управління кімнатами", "Управління зустрічами")
            .AddNewRow().AddButtons("Видалити зустріч");

        _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public async override Task HandleAnswer(string answer)
    {
        switch (answer)
        {
            case "Створити кімнату":
                _telegramBotContext.state = new RoomNameState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Спланувати зустріч":
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Управління кімнатами":
                _telegramBotContext.state = new RoomManagerState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;
            case "Управління зустрічами":
                _telegramBotContext.state = new MeetingManagerState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;

            case "Видалити зустріч":
                _telegramBotContext.state = new MeetingDeleteState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                break;

            default:
                _telegramBotContext.state = this;
                break;
        }
    }
}
