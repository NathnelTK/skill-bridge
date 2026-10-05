namespace TB.Domain.Entities;

public sealed class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<User> Users { get; set; } = new();
    public List<Job> Jobs { get; set; } = new();
}
