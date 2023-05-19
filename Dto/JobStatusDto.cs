namespace ConfigurationSaver_API.Dto
{
    public class JobStatusDto
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastRunTime { get; set; }
        public String LastJobState { get; set; }
        public DateTime? NextRunTime { get; set; }
    }
}
