using Microsoft.EntityFrameworkCore;
using ZoomRoom.Persistence.Models;

namespace ZoomRoom.Persistence
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<RoomUser> RoomUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ZoomRoom.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomUser>(builder =>
            {
                builder.HasKey(ru => new { ru.RoomId, ru.UserId });
                builder.HasOne(ru => ru.Room)
                    .WithMany(r => r.RoomUsers)
                    .HasForeignKey(ru => ru.RoomId);
                builder.HasOne(ru => ru.User)
                    .WithMany(u => u.RoomUsers)
                    .HasForeignKey(ru => ru.UserId);
            });
        }
    }
}