namespace Dto
{
    public class ScheduleTaskDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRun { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

    }
}