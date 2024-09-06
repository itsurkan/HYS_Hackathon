using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingDateState : State
{

    public MeetingDateState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть дату проведення зустрічі:\n (формат: рік.місяць.день година:хвилина)";
    }

    public override async void HandleAnswer(string answer)
    {

        if (_telegramBotContext == null) throw new Exception(nameof(_telegramBotContext));

        if(answer == "Назад")
        {
            _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
        }
        
        if(string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Дата зустрічі не може бути пустою ");
            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
        }
        else
        {
            _telegramBotContext.meetingData.MeetingDate = answer;
            
            
            if(_telegramBotContext.meetingData.IsFilled)
            {
                _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
            }


            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);  
        }
    }
}