using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;


public class MeetingCreatorState : State
{
    public MeetingCreatorState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть тему нового міту:";

        _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            if (answer == "Назад")
            {
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                return;
            }  if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Тема зустрічі не може бути пустою ");
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
            }
            else
            {
                _telegramBotContext.meetingData.Title = answer;

                if (_telegramBotContext.MeetingFormIsFilled)
                {
                    _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
                }
                _telegramBotContext.state = new MeetingRoomState(_telegramBotContext);
            }

        }
    }
}
