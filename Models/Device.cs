using ConfigurationSaver_API.Models;

namespace Models
{
    public abstract class Device
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String IpAddress { get; set; }
        public Credential Credential { get; set; }
        public ICollection<DeviceScheduleTask> DeviceScheduleTasks { get; set; }

        public virtual void RunBackup() { }

    }
}

