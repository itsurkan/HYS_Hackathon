namespace ZoomRoom.Persistence.Models;

public class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string ZoomLink { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
}
