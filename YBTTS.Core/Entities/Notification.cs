namespace YBTTS.Core.Entities;

public class Notification
{
    public int Id { get; set; }
    public int? StudentId { get; set; }
    public int? MaintenanceStaffId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "Request", "Badge", "Level", "Assignment"
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Student? Student { get; set; }
    public MaintenanceStaff? MaintenanceStaff { get; set; }
}
