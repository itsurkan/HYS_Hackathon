namespace ZoomRoom.Persistence.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }= String.Empty;
    public string Password { get; set; } = String.Empty;
    public ICollection<RoomUser> RoomUsers { get; set; } 
    public ICollection<Meeting> Meetings { get; set; } 
}
