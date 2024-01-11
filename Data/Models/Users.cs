
namespace bislerium.Data
{
    public class Users
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public Roles Role { get; set; }
        public bool HasInitialPassword { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
    }
}
