using System;
using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;


public class MeetingCreatorState : State
{
    public MeetingCreatorState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
    }

    public override async Task Initialize()
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Введіть тему нового міту:";

        await  _telegramBotContext!.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, textMessage, replyMarkup: keyboardMarkup);
    }

    public override async Task HandleAnswer(string answer)
    {
        if (_telegramBotContext is not null)
        {
            if (answer == "Назад")
            {
                _telegramBotContext.state = new MainMenu(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                return;
            }  if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Тема зустрічі не може бути пустою ");
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }
            else
            {
                _telegramBotContext.meetingData.Title = answer;

                if (_telegramBotContext.MeetingFormIsFilled)
                {
                    _telegramBotContext.state = new MeetingResultCheckState(_telegramBotContext);
                    await _telegramBotContext.state.Initialize();
                }
                _telegramBotContext.state = new MeetingRoomState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
            }

        }
    }
}
