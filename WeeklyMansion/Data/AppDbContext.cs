using Microsoft.EntityFrameworkCore;
using WeeklyMansion.Models;

namespace WeeklyMansion.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties
            => Set<Property>();

        public DbSet<Room> Rooms
            => Set<Room>();

        public DbSet<RoomStatus> RoomStatuses
            => Set<RoomStatus>();
    }
}
