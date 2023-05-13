using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ServerScheduleTask>().HasKey(sst => new {sst.ScheduleTaskId, sst.ServerId});
            builder.Entity<ServerScheduleTask>().HasOne(sst => sst.ScheduleTask).WithMany(st => st.ServerScheduleTasks).HasForeignKey(st => st.ScheduleTaskId);
            builder.Entity<ServerScheduleTask>().HasOne(sst => sst.Server).WithMany(st => st.ServerScheduleTasks).HasForeignKey(st => st.ServerId);
        }

        public DbSet<Server> Servers {get; set;}
        public DbSet<Credential> Credentials {get; set;}
        public DbSet<ScheduleTask> ScheduleTasks {get; set;}
        public DbSet<ServerScheduleTask> ServerScheduleTasks {get; set;}
    }
}