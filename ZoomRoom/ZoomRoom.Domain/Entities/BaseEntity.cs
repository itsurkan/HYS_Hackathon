namespace ZoomRoom.Domain.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
