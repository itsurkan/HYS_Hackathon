using Newtonsoft.Json;

namespace ZoomRoom.Domain.Responses
{
    public class MeetingsListResponse
    {
        [JsonProperty("meetings")]
        public List<MeetingResponse> Meetings { get; set; }
    }
}
