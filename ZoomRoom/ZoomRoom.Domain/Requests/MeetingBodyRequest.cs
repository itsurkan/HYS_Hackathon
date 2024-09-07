using ZoomRoom.Domain.Enums;

namespace ZoomRoom.Domain.Requestes
{
    public record MeetingBodyRequest(string topic, DateTime startTime, int duration, string agenda, UTCTimeZone timeZone);
}
