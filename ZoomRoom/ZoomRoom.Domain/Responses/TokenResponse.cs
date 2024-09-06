using Newtonsoft.Json;

namespace ZoomRoom.Domain.Responses
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

}
