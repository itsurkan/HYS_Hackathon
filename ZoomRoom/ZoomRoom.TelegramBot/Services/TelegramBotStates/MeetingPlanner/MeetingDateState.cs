using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomBuilder;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingDateState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть дату проведення зустрічі:\n (формат: рік.місяць.день година:хвилина)";

        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Дата зустрічі не може бути пустою ");
            _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
            await _telegramBotContext.state.Initialize();
        }
        else
        {
            if (answer == "Назад")
            {
                _telegramBotContext.state = new RoomCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                return;
            }

            try
            {
                _telegramBotContext.meetingData.ScheduledTime = DateTime.Parse(answer, CultureInfo.InvariantCulture);
            }
            catch
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Невірний формат дати");
                _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                return;
            }

            _telegramBotContext.state = _telegramBotContext.MeetingFormIsFilled
                ? new MeetingResultCheckState(_telegramBotContext)
                : new MeetingDurationState(_telegramBotContext);
            await _telegramBotContext.state.Initialize();
        }
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        return base.HandleCallbackQuery(callbackQuery);
    }
}
