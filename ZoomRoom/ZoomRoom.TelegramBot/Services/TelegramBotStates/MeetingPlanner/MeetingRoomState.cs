using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ZoomRoom.Persistence;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates.MeetingPlanner;

public class MeetingRoomState : State
{
    bool skipMessageHandling = false;

    public MeetingRoomState(TelegramBotContext telegramBotContext) :
        base(telegramBotContext)
    {

    }

    public override async Task Initialize()
    {

        keyboardMarkup = new ReplyKeyboardMarkup(true).AddButton("Назад");
        textMessage = "Оберіть кімнату для зустрічі:";


        if (_telegramBotContext.botClient is not null)
        {
            List<Room> rooms = await _telegramBotContext.roomService.GetAllRoomsWithUserAsync();

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup();
            foreach (Room room in rooms)
            {
                inlineKeyboard.AddButtons(room.Name);
            }

            await _telegramBotContext!.botClient!.SendTextMessageAsync(
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
                await _telegramBotContext.state.Initialize();
            }
            else if (answer == "Назад")
            {
                _telegramBotContext.state = new MeetingCreatorState(_telegramBotContext);
                await _telegramBotContext.state.Initialize();
                return;
            }
        }
    }

    public override async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {

        if (_telegramBotContext is not null)
        {
            var rooms = await _telegramBotContext.roomService.GetAllRoomsAsync();

            //Fix this
            using (var db = new SqliteDbContext(new DbContextOptions<SqliteDbContext>()))
            {
                List<RoomUser> r = db.RoomUsers.ToList();

                foreach (RoomUser roomUser in r)
                {
                    if (roomUser.UserId == _telegramBotContext.chatId)
                    {
                        rooms.Add(roomUser.Room);
                    }
                }
            }
            _telegramBotContext!.meetingData.RoomId = rooms.FirstOrDefault(r => r.Name == callbackQuery.Data).Id;
            _telegramBotContext.state = new MeetingDateState(_telegramBotContext);
            await _telegramBotContext.state.Initialize();

            skipMessageHandling = true;
            await _telegramBotContext.botClient!.AnswerCallbackQueryAsync(callbackQuery.Id);
        }
    }
}
