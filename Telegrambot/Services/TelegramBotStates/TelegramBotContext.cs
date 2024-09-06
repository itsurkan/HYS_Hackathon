using System;
using StudyBot.Services;
using Telegram.Bot;
using Telegrambot.Services.TelegramBotStates.MeatingPlanner;
using Telegrambot.Services.TelegramBotStates.RoomBuilder;

namespace Telegrambot.Services.TelegramBotStates;

public class TelegramBotContext
{
    public State state;
    
    public ITelegramBotClient? botClient = null;
    public long chatId = 0;

    public HttpClient? httpClient = new HttpClient();

    public MeetingData meetingData = new MeetingData();
    public RoomData roomData = new RoomData();

    public TelegramBotContext(ITelegramBotClient botClient, long chatId)
    {
        this.botClient = botClient;
        this.chatId = chatId;
        state = new MainMenu(this);
    }

}
