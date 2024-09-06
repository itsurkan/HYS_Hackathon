using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingDurationState : State
{
    public MeetingDurationState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть тривалість зустрічі:";
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if(answer == "Назад")
        {
            _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
        }

        if(string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Тривалість зустрічі не може бути пустою ");
            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
        }
        else
        {
            _telegramBotContext.meetingData.Duration = answer;

            
            if(_telegramBotContext.meetingData.IsFilled)
            {
                _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
            }

            _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);  
        }
    }

}
