using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingPasscodeState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть пароль зустрічі:";

        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Пароль зустрічі не може бути пустим ");
            _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
            await _telegramBotContext.state.Initialize();
        }

        if (answer == "Назад")
        {
            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
            await _telegramBotContext.state.Initialize();
            return;
        }

        _telegramBotContext.meetingData.Password = answer;

        _telegramBotContext.state = _telegramBotContext.MeetingFormIsFilled
            ? new MeetingResultCheckState(_telegramBotContext)
            : new MeetingTimezoneState(_telegramBotContext);
        await _telegramBotContext.state.Initialize();
    }
}
