using Hackathon.Domain.Enums;

namespace Hackathon.Domain.Requestes
{
    public record MeetingBodyRequest(string topic, DateTime startTime, int duration, string agenda, TimeZones timeZone);
}
