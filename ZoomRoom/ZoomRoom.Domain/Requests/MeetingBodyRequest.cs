using ZoomRoom.Domain.Enums;

namespace ZoomRoom.Domain.Requests
{
    public record MeetingBodyRequest(string topic, DateTime startTime, int duration, string agenda, UTCTimeZone timeZone);
}
