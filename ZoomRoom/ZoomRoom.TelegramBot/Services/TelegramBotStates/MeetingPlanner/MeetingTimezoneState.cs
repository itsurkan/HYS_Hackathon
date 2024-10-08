using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Domain.Enums;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingTimezoneState(TelegramBotContext telegramBotContext) : State(telegramBotContext)
{
    bool skipMessageHandling;

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Оберіть часовий пояс зустрічі:";

        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
        inlineKeyboard.AddButtons("UTC +12", "UTC +11", "UTC +10", "UTC +9").AddNewRow().AddButtons("UTC +8", "UTC +7", "UTC +6", "UTC +5").AddNewRow()
            .AddButtons("UTC +4", "UTC +3", "UTC +2", "UTC +1").AddNewRow().AddButtons("UTC 0").AddNewRow().AddButtons("UTC -1", "UTC -2", "UTC -3", "UTC -4").AddNewRow()
            .AddButtons("UTC -5", "UTC -6", "UTC -7", "UTC -8").AddNewRow().AddButtons("UTC -9", "UTC -10", "UTC- 11", "UTC -12").AddNewRow();

        await _telegramBotContext.botClient.SendTextMessageAsync(
            chatId: _telegramBotContext.chatId,
            text: "Оберіть часовий пояс зустрічі:",
            replyMarkup: inlineKeyboard
        );
    }

    public override async Task HandleAnswer(string answer)
    {
        if (skipMessageHandling) return;
        if (string.IsNullOrEmpty(answer))
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Часовий пояс зустрічі не може бути пустим ");
            _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
        }
        else if (answer == "Назад")
        {
            _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
        }
        else
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Оберіть часовий пояс із списку!");
            _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
        }

        await _telegramBotContext.state.Initialize();
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        var uTCTimeZone = callbackQuery.Data switch
        {
            "UTC +12" => UTCTimeZone.UTCPlus12,
            "UTC +11" => UTCTimeZone.UTCPlus11,
            "UTC +10" => UTCTimeZone.UTCPlus10,
            "UTC +9" => UTCTimeZone.UTCPlus9,
            "UTC +8" => UTCTimeZone.UTCPlus8,
            "UTC +7" => UTCTimeZone.UTCPlus7,
            "UTC +6" => UTCTimeZone.UTCPlus6,
            "UTC +5" => UTCTimeZone.UTCPlus5,
            "UTC +4" => UTCTimeZone.UTCPlus4,
            "UTC +3" => UTCTimeZone.UTCPlus3,
            "UTC +2" => UTCTimeZone.UTCPlus2,
            "UTC +1" => UTCTimeZone.UTCPlus1,
            "UTC 0" => UTCTimeZone.UTCZero,
            "UTC -1" => UTCTimeZone.UTCMinus1,
            "UTC -2" => UTCTimeZone.UTCMinus2,
            "UTC -3" => UTCTimeZone.UTCMinus3,
            "UTC -4" => UTCTimeZone.UTCMinus4,
            "UTC -5" => UTCTimeZone.UTCMinus5,
            "UTC -6" => UTCTimeZone.UTCMinus6,
            "UTC -7" => UTCTimeZone.UTCMinus7,
            "UTC -8" => UTCTimeZone.UTCMinus8,
            "UTC -9" => UTCTimeZone.UTCMinus9,
            "UTC -10" => UTCTimeZone.UTCMinus10,
            "UTC -11" => UTCTimeZone.UTCMinus11,
            "UTC -12" => UTCTimeZone.UTCMinus12,
            _ => UTCTimeZone.UTCZero
        };

        _telegramBotContext.meetingData.TimeZone = uTCTimeZone;
        _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
        await _telegramBotContext.state.Initialize();

        skipMessageHandling = true;

        await _telegramBotContext.botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
    }
}
