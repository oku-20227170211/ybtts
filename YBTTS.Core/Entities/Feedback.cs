namespace YBTTS.Core.Entities;

public class Feedback
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int StudentId { get; set; }
    public int Rating { get; set; } = 0; // 1-5 arası
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Request Request { get; set; } = null!;
    public Student Student { get; set; } = null!;
}
