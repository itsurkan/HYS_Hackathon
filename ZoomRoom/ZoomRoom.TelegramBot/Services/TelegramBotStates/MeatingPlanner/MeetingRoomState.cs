using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence.Models;

namespace Telegrambot.Services.TelegramBotStates.MeatingPlanner;

public class MeetingRoomState : State
{
    bool skipMessageHandling = false;

    public MeetingRoomState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {
        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Оберіть кімнату для зустрічі:";

        if (telegramBotContext.botClient is not null)
        {
            List<Room> rooms = telegramBotContext.roomService.GetAllRoomsWithUserAsync().GetAwaiter().GetResult()
                    .ToList();

            // todo handle no rooms

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
            foreach (Room room in rooms)
            {
                inlineKeyboard.AddButtons(room.Name);
            }

            _telegramBotContext!.botClient!.SendTextMessageAsync(
                chatId: _telegramBotContext.chatId,
                text: "Оберіть кімнату для зустрічі:",
                replyMarkup: inlineKeyboard
            );
        }

    }


    public override async Task HandleAnswer(string answer)
    {
        if (skipMessageHandling) return;

        if (_telegramBotContext is not null)
        {
            if (string.IsNullOrEmpty(answer))
            {
                await _telegramBotContext.botClient!.SendTextMessageAsync(_telegramBotContext.chatId, "Кімната для зустрічі не може бути пустою");
                _telegramBotContext.state = new MeetingRoomState(_telegramBotContext);
            }
            else if (answer == "Назад")
            {
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                return;
            }
        }
    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {

        if (_telegramBotContext is not null)
        {
            var rooms = _telegramBotContext.roomService.GetAllRoomsWithUserAsync().GetAwaiter().GetResult()
                .ToList();

            _telegramBotContext!.meetingData.RoomId = rooms.FirstOrDefault(r => r.Name == callbackQuery.Data).Id;
            _telegramBotContext.state = new MeetingDateState(_telegramBotContext);

            skipMessageHandling = true;
            await _telegramBotContext.botClient!.AnswerCallbackQueryAsync(callbackQuery.Id);
        }
    }
}
