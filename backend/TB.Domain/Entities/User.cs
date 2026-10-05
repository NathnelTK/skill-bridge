    namespace TB.Domain.Entities;

    public sealed class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public List<Skill> Skills { get; set; } = new();
        public List<Job> PostedJobs { get; set; } = new();
        public List<Application> Applications { get; set; } = new();
    }
