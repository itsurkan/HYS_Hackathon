using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingManager;

public class MeetingInteractiveState : State
{
    int meetingId;

    public MeetingInteractiveState(TelegramBotContext telegramBotContext, int meetingId) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).
        AddButtons("Видалити", "Зупинити").AddNewRow().AddButtons("Назад");
    }

    public override void HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            switch (answer)
            {
                case "Назад":
                    _telegramBotContext.state = new MeetingManagerState(_telegramBotContext);
                    break;
                case "Видалити":
                    DeleteNote(meetingId);
                    break;
                case "Зупинити":
                    StopMeeting(meetingId);
                    break;
                default:
                    _telegramBotContext.state = this;
                    break;
            }
        }
    }

    private void DeleteNote(int id)
    {

    }

    private void StopMeeting(int id)
    {

    }

    public void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        throw new NotImplementedException();
    }
}
