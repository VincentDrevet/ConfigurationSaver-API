namespace Models
{
    public class Server
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String IpAddress { get; set; }
        public Credential Credential { get; set; }
        public ICollection<ServerScheduleTask> ServerScheduleTasks { get; set; }
    }
}

