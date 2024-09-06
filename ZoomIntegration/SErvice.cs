using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Newtonsoft.Json;

public class ZoomService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _accountId;
    private readonly string _redirectUri;

    public ZoomService( string clientId, string clientSecret, string accountId)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _accountId = accountId;
    }

    public string GetAuthorizationUrl()
    {
        return $"https://zoom.us/oauth/authorize?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}";
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        var response =  await _httpClient.PostAsync($"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={_accountId}", null);

        Console.WriteLine(response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
        return tokenResponse.AccessToken;
    }

    public async Task<string> CreateMeetingAsync(string accessToken, string topic, DateTime startTime, int duration)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/v2/users/me/meetings");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var meetingDetails = new
        {
            topic = topic,
            type = 2,
            start_time = startTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            duration = duration,
            timezone = "UTC",
            agenda = "Discuss project updates"
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(meetingDetails), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var meetingResponse = JsonConvert.DeserializeObject<MeetingResponse>(content);
        return meetingResponse.JoinUrl;
    }
}

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}

public class MeetingResponse
{
    [JsonProperty("join_url")]
    public string JoinUrl { get; set; }
}
