namespace Models
{
    public class DeviceScheduleTask
    {
        public Guid DeviceId { get; set; }
        public Guid ScheduleTaskId { get; set; }
        public Device Device { get; set; }
        public ScheduleTask ScheduleTask { get; set; }
    }
}