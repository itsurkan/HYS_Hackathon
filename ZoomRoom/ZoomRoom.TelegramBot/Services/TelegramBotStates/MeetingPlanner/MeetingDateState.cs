using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.TelegramBot.Services.TelegramBotStates.RoomBuilder;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingDateState : State
{

    public MeetingDateState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {

    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть дату проведення зустрічі:\n (формат: рік.місяць.день година:хвилина)";

        await _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);

        return;
    }

    public override async Task HandleAnswer(string answer)
    {

        if (_telegramBotContext is not null)
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
                        await _telegramBotContext.state.Initialize();
                        return;
                    }


                    if (_telegramBotContext.MeetingFormIsFilled)
                    {
                        _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
                        await _telegramBotContext.state.Initialize();
                        return;
                    }
                    else
                    {
                        _telegramBotContext.state = new MeetingDurationState(_telegramBotContext);
                        await _telegramBotContext.state.Initialize();
                    }



                }
            }
        }
    }

}
