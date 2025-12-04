namespace YBTTS.Core.Entities;

/// <summary>
/// Öğrenci-Rozet ilişki tablosu (Many-to-Many)
/// </summary>
public class StudentBadge
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int BadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Student Student { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}
