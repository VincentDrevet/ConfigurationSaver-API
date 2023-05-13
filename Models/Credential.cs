namespace Models
{
    public class Credential
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public ICollection<Server> Servers { get; set; }
    }
}