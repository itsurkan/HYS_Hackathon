namespace ZoomRoom.Persistence.Models;

public class Reminder
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string TaskName{ get; set; }
    public DateTime TaskTime{ get; set; }
    public bool CompliteStatus { get; set; }
}
