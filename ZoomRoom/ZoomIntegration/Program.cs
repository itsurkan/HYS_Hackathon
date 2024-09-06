using ZoomRoom.Domain.Requestes;

class Program
{
    private static readonly string clientId = "xlo8rJF9SS2EWoUf5THg2w";
    private static readonly string clientSecret = "Zd7tX1JJqmgfjm67PN9KHYamuvn7XASw";
    private static readonly string accountId = "Yov_JrovR0uHWCi19BiM3w";

    static async Task Main(string[] args)
    {
        var zoomService = new ZoomService(clientId, clientSecret, accountId);
        var accessToken = await zoomService.GetAccessTokenAsync();
        //var joinUrl = await zoomService.CreateMeetingAsync(accessToken, "New Test 2 ", DateTime.UtcNow.AddHours(1), 30);
        //Console.WriteLine($"Meeting created successfully! Join URL: {joinUrl}");

        var upcomingMeetings = await zoomService.GetUpcomingMeetingsAsync(accessToken);

        foreach (var meeting in upcomingMeetings)
        {
            Console.WriteLine($"Тема: {meeting.Topic}, Время начала: {meeting.StartTime}, Ссылка для подключения: {meeting.JoinUrl}");
        }

        var meetingId = "81192177971";
        bool isUpdated = await zoomService.UpdateMeetingAsync(accessToken, meetingId, new MeetingBodyRequest("Updated555", DateTime.UtcNow.AddHours(1), 45, "nothing", ZoomRoom.Domain.Enums.TimeZones.UTC));

        if (isUpdated)
        {
            Console.WriteLine($"Встреча с ID {meetingId} была успешно обновлена.");
        }
        else
        {
            Console.WriteLine($"Ошибка при обновлении встречи с ID {meetingId}.");
        }

        upcomingMeetings = await zoomService.GetUpcomingMeetingsAsync(accessToken);

        foreach (var meeting in upcomingMeetings)
        {
            Console.WriteLine($"Тема: {meeting.Topic}, Время начала: {meeting.StartTime}, Ссылка для подключения: {meeting.JoinUrl}");
        }
    }
}
