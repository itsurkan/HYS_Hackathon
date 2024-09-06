class Program
{
    private static readonly string clientId = "oIcNUkH_QuWPUdivUkUvYw";
    private static readonly string clientSecret = "Ici1FoUNwZjoxJBvU8m5OQZ0EvilOB1X";
    private static readonly string accountId = "27t3IPXMTmus_wEvxgkEjA";

    static async Task Main(string[] args)
    {
        var zoomService = new ZoomService(clientId, clientSecret, accountId);
        var accessToken = await zoomService.GetAccessTokenAsync();
        var joinUrl = await zoomService.CreateMeetingAsync(accessToken, "New Test 2 ", DateTime.UtcNow.AddHours(1), 30);
        Console.WriteLine($"Meeting created successfully! Join URL: {joinUrl}");
    }
}
