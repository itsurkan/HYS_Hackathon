using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingRoomState : State
{
    public MeetingRoomState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Оберіть кімнату для зустрічі:";
    }

    public  void CheckAccess(string room)
    {

    }

    public override async void HandleAnswer(string answer)
    {
        if (_telegramBotContext == null) throw new ArgumentNullException(nameof(_telegramBotContext));

        if (answer == "Назад")
        {
            _telegramBotContext.state = new MeetingTimezoneState(_telegramBotContext);
        }

        if (answer is null || answer == "")
        {
            await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната для зустрічі не може бути пустою");
            _telegramBotContext.state = new MeetingRoomState(_telegramBotContext);
        }
        else
        {
            CheckAccess(answer);

            _telegramBotContext.meetingData.room = answer;
            _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
        }
    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        await _telegramBotContext.botClient.SendTextMessageAsync(_telegramBotContext.chatId, 
                                        "debug");
    }
}
