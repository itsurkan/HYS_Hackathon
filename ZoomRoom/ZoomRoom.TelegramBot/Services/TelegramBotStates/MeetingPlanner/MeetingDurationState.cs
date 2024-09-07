using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingDurationState : State
{
    public MeetingDurationState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть тривалість зустрічі (в хвилинах):";

        _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);

    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            if (answer == "Назад")
            {
                _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
                return;
            }

            if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Тривалість зустрічі не може бути пустою ");
                _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
                return;
            }

            if (int.TryParse(answer, out var duration))
            {
                _telegramBotContext.meetingData.Duration = duration;

                _telegramBotContext.state = _telegramBotContext.MeetingFormIsFilled
                    ? new MeetingResultCheckState(_telegramBotContext)
                    : new MeetingPasscodeState(_telegramBotContext);
            }
            else
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Невірний формат тривалості");
                _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
            }
        }
    }

}
