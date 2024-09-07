namespace Telegrambot.Services.ReceiverService;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken cancellationToken);
}