using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Persistence.Models;

public class Meeting : BaseEntity
{
    public string Title { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string ZoomLink { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
}
