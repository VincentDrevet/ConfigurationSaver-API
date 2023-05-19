using ConfigurationSaver_API.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<DeviceScheduleTask>().HasKey(sst => new {sst.ScheduleTaskId, sst.DeviceId});
            builder.Entity<DeviceScheduleTask>().HasOne(sst => sst.ScheduleTask).WithMany(st => st.DeviceScheduleTasks).HasForeignKey(st => st.ScheduleTaskId);
            builder.Entity<DeviceScheduleTask>().HasOne(sst => sst.Device).WithMany(st => st.DeviceScheduleTasks).HasForeignKey(st => st.DeviceId);
        }

        public DbSet<Device> Devices {get; set;}
        public DbSet<EsxiServer> EsxiServers{ get; set; }
        public DbSet<Credential> Credentials {get; set;}
        public DbSet<ScheduleTask> ScheduleTasks {get; set;}
        public DbSet<DeviceScheduleTask> DeviceScheduleTasks {get; set;}
    }
}