using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingTimezoneState : State
{
    public MeetingTimezoneState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Оберіть часовий пояс зустрічі:";

        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
        inlineKeyboard.AddButtons("UTC-12", "UTC-11", "UTC-10", "UTC-9:30").AddNewRow().
                        AddButtons("UTC-9", "UTC-8", "UTC-7", "UTC-6").AddNewRow().
                        AddButtons("UTC-5", "UTC-4", "UTC-3:30", "UTC-3").AddNewRow().
                        AddButtons("UTC-2", "UTC-1", "UTC-00", "UTC-01").AddNewRow().
                        AddButtons( "UTC-02", "UTC-03", "UTC-03:30", "UTC-04").AddNewRow().
                        AddButtons("UTC-04:30", "UTC-05","UTC-05:30", "UTC-05:45").AddNewRow().
                        AddButtons("UTC-06", "UTC-06:30", "UTC-07", "UTC-08").AddNewRow().
                        AddButtons("UTC-08:30", "UTC-08:45", "UTC-09", "UTC-09:30").AddNewRow().
                        AddButtons("UTC-10", "UTC-10:30", "UTC-11", "UTC-12").AddNewRow().
                        AddButtons("UTC-12:45", "UTC-13", "UTC-14");

        _telegramBotContext.botClient.SendTextMessageAsync(
                chatId: _telegramBotContext.chatId,
                text: "Оберіть часовий пояс зустрічі:",
                replyMarkup: inlineKeyboard
            );

    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if (answer == "Назад")
        {
            _telegramBotContext.state = new MeetingPasscodeState(_telegramBotContext);
        }

        if (answer is null || answer == "")
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Часовий пояс зустрічі не може бути пустим ");
            _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
        }
        else
        {
            _telegramBotContext.meetingData.timezone = answer;
            _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
        }
    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        _telegramBotContext.meetingData.timezone = callbackQuery.Data;
        _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);

        await _telegramBotContext.botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
    }


}
