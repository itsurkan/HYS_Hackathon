using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Persistence.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public long? ChatId { get; set; }
    public ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();
}
