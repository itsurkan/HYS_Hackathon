using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Persistence.Models;

public class Room : BaseEntity
{
    public string Name { get; set; }
    public ICollection<RoomUser> RoomUsers { get; set; }
    public ICollection<Meeting> Meetings { get; set; }
}
