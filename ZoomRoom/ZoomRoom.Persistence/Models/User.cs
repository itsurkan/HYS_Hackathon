using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Persistence.Models;

public class User : BaseEntity
{
    public long Id { get; set; } // Telegram user ID
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<RoomUser> RoomUsers { get; set; }
}
