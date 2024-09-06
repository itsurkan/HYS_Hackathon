using System;
using System.Reflection.Metadata;
using StudyBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingData{
    public string MeetingName { get; set; } = "";
    public string MeetingDate { get; set; } = "";
    public string Duration { get; set; } = "";
    public string Passcode { get; set; } = "";
    public string Timezone { get; set; } = "";
    public string Room { get; set; } = "";
    public bool IsFilled { get; set; } = false; 
}

public class MeetingCreatorState : State
{
    public MeetingCreatorState(TelegramBotContext telegramBotContext) : 
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть тему нового міту:";
    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if(answer == "Назад")
        {
            _telegramBotContext.state = new MainMenu(_telegramBotContext);  
        }

        if(string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Тема зустрічі не може бути пустою ");
            _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
        }
        else
        {
            _telegramBotContext.meetingData.MeetingName = answer;

            if(_telegramBotContext.meetingData.IsFilled)
            {
                _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
            }
            _telegramBotContext.state = new MeetingRoomState(_telegramBotContext);  
        }
    }
}
