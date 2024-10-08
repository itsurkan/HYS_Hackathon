using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using ZoomRoom.Domain.Requests;
using ZoomRoom.Domain.Responses;
using ZoomRoom.Services.Interfaces;

namespace ZoomRoom.Services.Services;

public class ZoomService(IOptions<ZoomSettings> zoomSettings) : IZoomService, IDisposable
{
    private ZoomSettings ZoomSettings { get; } = zoomSettings.Value;
    private readonly HttpClient _httpClient = new ();


    public async Task<string> GetAccessTokenAsync()
    {
        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ZoomSettings.ClientId}:{ZoomSettings.ClientSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        var response = await _httpClient.PostAsync($"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={ZoomSettings.AccountId}", null);

        var result = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
        return tokenResponse!.AccessToken;
    }

    public async Task<MeetingResponse?> CreateMeetingAsync(string accessToken, MeetingBodyRequest requestBody)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/v2/users/me/meetings");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var meetingDetails = new
        {
            topic = requestBody.topic,
            type = 2,
            start_time = requestBody.startTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            duration = requestBody.duration,
            timezone = requestBody.timeZone,
            agenda = requestBody.agenda,
            settings = new
            {
                join_before_host = true,
            }
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(meetingDetails), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var meetingResponse = JsonConvert.DeserializeObject<MeetingResponse>(content);
        return meetingResponse;
    }

    public async Task<List<MeetingResponse>> GetUpcomingMeetingsAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.zoom.us/v2/users/me/meetings?type=upcoming");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var meetingsResponse = JsonConvert.DeserializeObject<MeetingsListResponse>(content);
        return meetingsResponse!.Meetings;
    }

    public async Task<bool> DeleteMeetingAsync(string accessToken, string meetingId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"https://api.zoom.us/v2/meetings/{meetingId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateMeetingAsync(string accessToken, string meetingId, MeetingBodyRequest requestBody)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"https://api.zoom.us/v2/meetings/{meetingId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var meetingDetails = new
        {
            topic = requestBody.topic,
            start_time = requestBody.startTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            duration = requestBody.duration,
            timezone = requestBody.timeZone.ToString(),
            agenda = requestBody.agenda
        };


        request.Content = new StringContent(JsonConvert.SerializeObject(meetingDetails), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> StartMeetingAsync(string accessToken, long meetingId)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"https://api.zoom.us/v2/meetings/{meetingId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var statusUpdate = new
        {
            action = "start"
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(statusUpdate), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
