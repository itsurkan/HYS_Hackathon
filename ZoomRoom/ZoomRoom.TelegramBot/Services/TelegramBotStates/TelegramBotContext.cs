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

    public State state;

    public ITelegramBotClient? botClient;
    public long chatId;

    public Meeting meetingData = new ();
    public bool MeetingFormIsFilled { get; set; }

    public Room roomData = new Room();

    public TelegramBotContext Init(ITelegramBotClient botClient, long chatId)
    {
        this.botClient = botClient;
        this.chatId = chatId;
        state = new MainMenu(this);
        return this;
    }
}
