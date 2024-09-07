using ZoomRoom.Domain.Enums;

namespace ZoomRoom.Persistence.Models;

public class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public DateTime ScheduledTime { get; set; }
    public string Password { get; set; } = String.Empty;
    public UTCTimeZone TimeZone { get; set; }
    public int Duration { get; set; }
    public string ZoomLink { get; set; } = String.Empty;
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
}
