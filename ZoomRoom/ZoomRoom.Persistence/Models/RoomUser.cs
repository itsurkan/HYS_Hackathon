using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Persistence.Models;

public class RoomUser : BaseEntity
{
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}
