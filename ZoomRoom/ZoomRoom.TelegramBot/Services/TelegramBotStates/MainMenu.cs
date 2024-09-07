using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomManager;

namespace TelegramBot.Services;

public class MainMenu : State
{
    public MainMenu(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        textMessage = "Вітаємо в ZoomRoom! Що бажаєте зробити?";
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButtons("Створити кімнату", "Спланувати зустріч").
            AddNewRow().AddButtons("Управління кімнатами", "Управління зустрічами");

        _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);

    }

    public override Task HandleAnswer(string answer)
    {
        if (_telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(_telegramBotContext));
        }

        switch (answer)
        {
            case "Створити кімнату":
                _telegramBotContext.state = new RoomNameState(_telegramBotContext);
                break;
            case "Спланувати зустріч":
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                break;
            case "Управління кімнатами":
                _telegramBotContext.state = new RoomManagerState(_telegramBotContext);
                break;
            case "Управління зустрічами":
                _telegramBotContext.state = new MeetingManagerState(_telegramBotContext);
                break;

            default:
                _telegramBotContext.state = this;
                break;
        }

        return Task.CompletedTask;
    }
}
