using YBTTS.Core.Enums;

namespace YBTTS.Core.Entities;

public class Request
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RoomNo { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public int? AssignedToStaffId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    
    // Navigation properties
    public Student? Student { get; set; }
    public MaintenanceStaff? AssignedToStaff { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}