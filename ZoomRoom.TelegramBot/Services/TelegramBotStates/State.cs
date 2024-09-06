using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;

namespace Telegrambot.Services.TelegramBotStates;

public class State
{
    protected ReplyKeyboardMarkup _keyboardMarkup;

    public ReplyKeyboardMarkup keyboardMarkup
    {
        get => _keyboardMarkup;
        set => _keyboardMarkup = value;
    }

    protected TelegramBotContext? _telegramBotContext;

    private string _textMessage;
    public string textMessage
    {
        get => _textMessage ?? "empty message"; 
        set => _textMessage = value; 
    }
    

    public State(TelegramBotContext telegramBotContext)
    {
        if(telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(telegramBotContext));
        }
        
        _telegramBotContext = telegramBotContext;
        _keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("NOT IMPLEMENTED");        
    }

    public virtual void HandleAnswer(string answer)
    {
        if(_telegramBotContext == null)
        {
            throw new ArgumentNullException(nameof(_telegramBotContext));
        }

        switch(answer)
        {
            case "NOT IMPLEMENTED":
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                break;
            default:
                _telegramBotContext.state = this;
                break;
        }

    }

    public virtual void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        throw new Exception("Not callback query handler is called!!");
    }
}
