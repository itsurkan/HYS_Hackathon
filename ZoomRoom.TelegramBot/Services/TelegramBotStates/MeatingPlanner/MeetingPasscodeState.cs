using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingPasscodeState : State
{
    public MeetingPasscodeState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть пароль зустрічі:";
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if(answer == "Назад")
        {
            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
        }

        if(answer is null || answer == "")
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Пароль зустрічі не може бути пустим ");
            _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
        }
        else
        {
            _telegramBotContext.meetingData.passcode = answer;

            if(_telegramBotContext.meetingData.isFilled)
            {
                _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
            }

            _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);  
        }
    }
}
