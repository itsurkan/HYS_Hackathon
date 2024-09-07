using System;
using Telegram.Bot;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;
using TelegramBot.Services;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.Services;

namespace Telegrambot.Services.TelegramBotStates;

public class TelegramBotContext
{
    public State state;
    
    public ITelegramBotClient? botClient = null;
    public long chatId;

    public Meeting meetingData = new Meeting();
    public bool MeetingFormIsFilled { get; set; }

    public Room roomData = new Room();

    public UserService userService;
    public RoomService roomService;
    public MeetingService meetingService;
    //public ZoomService zoomService;    

    public TelegramBotContext(ITelegramBotClient botClient, long chatId, UserService userService, RoomService roomService, MeetingService meetingService)
    {
        this.botClient = botClient;
        this.chatId = chatId;
        this.userService = userService;
        this.roomService = roomService;
        this.meetingService = meetingService;
      //  this.zoomService = zoomService;
        state = new MainMenu(this);
    }

}
