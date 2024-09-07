using Microsoft.Extensions.Options;
using MudBlazor.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegrambot.Services;
using Telegrambot.Services.ReceiverService;
using ZoomRoom.Persistence;
using ZoomRoom.Services.PersistenceServices.Impl;
using ZoomRoom.TelegramBot;
using ZoomRoom.TelegramBot.Services;
using ZoomRoom.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();


//builder.Services.AddSingleton<IZoomService, ZoomService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RoomService>();
builder.Services.AddTransient<MeetingService>();

builder.Services.AddDbContext<SqliteDbContext>();


// Add telegram related services
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));


//TODO: Resolve this issue
builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
builder.Services.AddTransient<IReceiverService, ReceiverService>();
builder.Services.AddHostedService<PollingService>();

builder.Services.AddHttpClient("telegram_bot_client").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        BotSettings? botSettings = sp.GetService<IOptions<BotSettings>>()?.Value;

        if (botSettings is null)
        {
            throw new InvalidOperationException("Bot settings not found");
        }

        TelegramBotClientOptions options = new(botSettings.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
