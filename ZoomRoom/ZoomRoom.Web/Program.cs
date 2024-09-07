using MudBlazor.Services;
using ZoomRoom.Web.Data;
using Telegram.Bot.Polling;
using Telegrambot.Services;
using Telegrambot.Services.ReceiverService;
using ZoomRoom.TelegramBot;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using ZoomRoom.Persistence;
using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();

builder.Services.AddDbContext<SqliteDbContext>();

//builder.Services.AddSingleton<IZoomService, ZoomService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<MeetingService>();

// Add telegram related services
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));


//TODO: Resolve this issue
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
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
