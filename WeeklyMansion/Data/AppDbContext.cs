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

        public DbSet<Reservation> Reservations
            => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasIndex(x => new { x.PropertyId, x.RoomNo })
                .IsUnique();

            modelBuilder.Entity<RoomStatus>()
                .HasIndex(x => new { x.RoomId, x.TargetDate })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
