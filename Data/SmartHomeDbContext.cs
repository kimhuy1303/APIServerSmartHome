
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;
namespace APIServerSmartHome.Data
{
    public class SmartHomeDbContext : DbContext
    {
        public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<PowerDevice> PowerDevices { get; set; }
    }
}
