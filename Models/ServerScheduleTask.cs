namespace Models
{
    public class ServerScheduleTask
    {
        public Guid ServerId { get; set; }
        public Guid ScheduleTaskId { get; set; }
        public Server Server { get; set; }
        public ScheduleTask ScheduleTask { get; set; }
    }
}