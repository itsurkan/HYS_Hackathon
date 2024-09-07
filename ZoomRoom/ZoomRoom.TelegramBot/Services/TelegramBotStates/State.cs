using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates;

public class State
{
    protected ReplyKeyboardMarkup _keyboardMarkup;

    public ReplyKeyboardMarkup keyboardMarkup
    {
        get => _keyboardMarkup;
        set => _keyboardMarkup = value;
    }

    protected TelegramBotContext _telegramBotContext;

    private string _textMessage;
    public string textMessage
    {
        get => _textMessage ?? "empty message";
        set => _textMessage = value;
    }


    public State(TelegramBotContext telegramBotContext)
    {
        if (telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(telegramBotContext));
        }

        _telegramBotContext = telegramBotContext;
        _keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("NOT IMPLEMENTED");

        // _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);

    }

    public virtual Task Initialize()
    {
        return Task.CompletedTask;
    }

    public virtual Task HandleAnswer(string answer)
    {
        if (_telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(_telegramBotContext));
        }

        switch (answer)
        {
            case "NOT IMPLEMENTED":
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                break;
            default:
                _telegramBotContext.state = this;
                break;
        }

        return Task.CompletedTask;
    }

    public virtual void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        throw new Exception("Not callback query handler is called!!");
    }
}
