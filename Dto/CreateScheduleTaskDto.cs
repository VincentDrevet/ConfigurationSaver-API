namespace ConfigurationSaver_API.Dto
{
    public class CreateScheduleTaskDto
    {
        public String Name { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
