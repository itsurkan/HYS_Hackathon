namespace ZoomRoom.TelegramBot.Services.ReceiverService;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken cancellationToken);
}