﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using ZoomRoom.Bot.Host;
using ZoomRoom.Persistence;
using ZoomRoom.Services;
using ZoomRoom.Services.Interfaces;
using ZoomRoom.Services.Services;
using ZoomRoom.TelegramBot.Services;
using ZoomRoom.TelegramBot.Services.ReceiverService;
using ZoomRoom.TelegramBot.Services.TelegramBotStates;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register Bot configuration
        services.Configure<BotSettings>(context.Configuration.GetSection("BotSettings"));
        services.Configure<ZoomSettings>(context.Configuration.GetSection("ZoomSettings"));

        // Register named HttpClient to benefits from IHttpClientFactory
        // and consume it with ITelegramBotClient typed client.
        // More read:
        //  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients
        //  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotSettings? botConfiguration = sp.GetService<IOptions<BotSettings>>()?.Value;
                ArgumentNullException.ThrowIfNull(botConfiguration);
                TelegramBotClientOptions options = new(botConfiguration.BotToken);
                return new TelegramBotClient(options, httpClient);
            });
        services.AddDbContext<SqliteDbContext>(options => options.UseSqlite("Data Source=../ZoomRoom.db"));
        services.AddScoped<IUpdateHandler, UpdateHandler>();
        services.AddHostedService<PollingService>();
        services.AddScoped<IReceiverService, ReceiverService>();
        services.AddScoped<TelegramBotContext>();
        services.AddScoped<IZoomService, ZoomService>();
        services.AddPersistenceServices();
    })
    .Build();

await host.RunAsync();
