using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingPasscodeState : State
{
    public MeetingPasscodeState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть пароль зустрічі:";

       await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {

            if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Пароль зустрічі не може бути пустим ");
                _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }

            if (answer == "Назад")
            {
                _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                return;
            }
            else
            {
                _telegramBotContext.meetingData.Password = answer;

                if (_telegramBotContext.MeetingFormIsFilled)
                {
                    _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                    return;
                }
                else _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }

        }

    }
}
