using YBTTS.Core.Enums;

namespace YBTTS.Core.Entities;

public class Request
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int? SatisfactionScore { get; set; }
    
    // Navigation property
    public Student? Student { get; set; }
}
