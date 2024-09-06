using Newtonsoft.Json;

namespace Hackathon.Domain.Responses
{
    public class MeetingsListResponse
    {
        [JsonProperty("meetings")]
        public List<MeetingResponse> Meetings { get; set; }
    }
}
