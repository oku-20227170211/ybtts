namespace YBTTS.Core.Entities;

public class Badge
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Criteria { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public int RequiredPoints { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<StudentBadge> StudentBadges { get; set; } = new List<StudentBadge>();
}
