namespace ZoomRoom.Persistence.Models;

public class RoomUser
{
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}