using Newtonsoft.Json;

namespace Hackathon.Domain.Responses
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

}
