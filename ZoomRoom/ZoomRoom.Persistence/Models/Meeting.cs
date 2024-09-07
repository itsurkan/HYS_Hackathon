using ZoomRoom.Domain.Entities;
using ZoomRoom.Domain.Enums;

namespace ZoomRoom.Persistence.Models;

public class Meeting : BaseEntity
{

    public string Title { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string Password { get; set; } = String.Empty;
    public UTCTimeZone TimeZone { get; set; }
    public int Duration { get; set; }
    public string ZoomLink { get; set; } = String.Empty;
    public long ZoomMeetingId { get; set; }
    public long RoomId { get; set; }
    public Room Room { get; set; } = null!;
}
