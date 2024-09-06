using Newtonsoft.Json;

namespace ZoomRoom.Domain.Responses
{
    public class MeetingResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("join_url")]
        public string JoinUrl { get; set; }
    }
}
