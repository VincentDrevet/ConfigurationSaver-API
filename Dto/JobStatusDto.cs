namespace ConfigurationSaver_API.Dto
{
    public class JobStatusDto
    {
        public String Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastRunTime { get; set; }
        public String LastJobState { get; set; }
        public DateTime? NextRunTime { get; set; }
    }
}
