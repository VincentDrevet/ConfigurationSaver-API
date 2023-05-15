namespace ConfigurationSaver_API.Dto
{
    public class UpdateCredentialDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }

    }
}
