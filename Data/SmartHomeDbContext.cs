﻿
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
        public DbSet<RFIDCard> RFIDCards { get; set; }
        public DbSet<UserFaces> UserFaces { get; set; }
        public DbSet<UserDevices> UserDevices { get; set; }
        public DbSet<OperateTimeWorking> OperateTimeWorkings { get; set; }
        public DbSet<IrrigationSchedule> IrrigationSchedules { get; set; }
        public DbSet<TempHumidValue> TempHumidValues { get; set; }
        public DbSet<SoilHumidity> SoilHumidities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDevices>()
                .HasKey(e => new { e.UserId, e.DeviceId});
            modelBuilder.Entity<UserDevices>()
                .HasOne(e => e.Device)
                .WithMany(e => e.UserDevices)
                .HasForeignKey(e => e.DeviceId);
            modelBuilder.Entity<UserDevices>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserDevices)
                .HasForeignKey(e => e.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
