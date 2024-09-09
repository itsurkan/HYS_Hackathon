using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates;

public class State
{
    protected ReplyKeyboardMarkup keyboardMarkup;

    protected TelegramBotContext _telegramBotContext;

    protected string textMessage;

    protected State(TelegramBotContext telegramBotContext)
    {
        ArgumentNullException.ThrowIfNull(telegramBotContext);

        _telegramBotContext = telegramBotContext;
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("NOT IMPLEMENTED");

        // _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public virtual Task Initialize()
    {
        return Task.CompletedTask;
    }

    public virtual Task HandleAnswer(string answer)
    {
        _telegramBotContext.state = answer switch
        {
            "NOT IMPLEMENTED" => new MainMenu(_telegramBotContext),
            _ => this
        };

        return Task.CompletedTask;
    }

    public virtual Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        throw new NotImplementedException("Not callback query handler is called!!");
    }
}
