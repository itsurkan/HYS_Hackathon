using MudBlazor.Services;
using ZoomRoom.Bot.Host;
using ZoomRoom.IRepository.Implementation.Repositories;
using ZoomRoom.Persistence;
using ZoomRoom.Repository.Implementation.Repositories;
using ZoomRoom.Services.PersistenceServices;
using ZoomRoom.Services.PersistenceServices.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();


builder.Services.AddDbContext<SqliteDbContext>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddScoped<IRoomService, RoomService>();




// Add telegram related services
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));




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
