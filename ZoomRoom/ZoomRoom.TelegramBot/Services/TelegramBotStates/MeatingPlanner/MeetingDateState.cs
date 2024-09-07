using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingDateState : State
{

    public MeetingDateState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть дату проведення зустрічі:\n (формат: рік.місяць.день година:хвилина)";

        _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);

    }

    public override async Task HandleAnswer(string answer)
    {

        if (_telegramBotContext is not null)
        {
            if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Дата зустрічі не може бути пустою ");
                _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
            }
            else
            {
                if (answer == "Назад")
                {
                    _telegramBotContext.state = new RoomCreatorState(_telegramBotContext);
                    return;
                }
                else
                {
                    try
                    {
                        _telegramBotContext.meetingData.ScheduledTime = DateTime.Parse(answer);
                    }
                    catch
                    {
                        await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Невірний формат дати");
                        _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
                        return;
                    }


                    if (_telegramBotContext.MeetingFormIsFilled)
                    {
                        _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
                        return;
                    }
                    else _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);

                }
            }
        }
    }

}
