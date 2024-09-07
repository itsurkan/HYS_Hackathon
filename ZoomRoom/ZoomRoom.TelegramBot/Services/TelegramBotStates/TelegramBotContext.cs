using System;
using Telegram.Bot;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;
using TelegramBot.Services;
using ZoomRoom.Persistence.Models;
using ZoomRoom.Services.PersistenceServices;
using ZoomRoom.Services.PersistenceServices.Impl;
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

    public IUserService userService;
    public IRoomService roomService;
    public IMeetingService meetingService;
    //public ZoomService zoomService;

    public TelegramBotContext(ITelegramBotClient botClient, long chatId, IUserService userService, IRoomService roomService, IMeetingService meetingService)
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
