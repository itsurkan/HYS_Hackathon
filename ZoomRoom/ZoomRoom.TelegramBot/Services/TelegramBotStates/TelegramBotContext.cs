using Telegram.Bot;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.PersistenceServices;

namespace ZoomRoom.TelegramBot.Services.TelegramBotStates;

public class TelegramBotContext(IUserService userService, IRoomService roomService, IMeetingService meetingService, IZoomService zoomService)
{
    public IUserService userService { get; } = userService;
    public IRoomService roomService { get; } = roomService;
    public IMeetingService meetingService { get; } = meetingService;

    public IZoomService zoomService { get; } = zoomService;

    public bool MeetingFormIsFilled { get; set; }

    public ITelegramBotClient botClient;

    public State state;

    public long chatId;

    public Meeting meetingData = new ();

    public Room roomData = new ();

    public TelegramBotContext Init(ITelegramBotClient client, long userChatId)
    {
        botClient = client;
        chatId = userChatId;
        state = new MainMenu(this);
        return this;
    }
}
