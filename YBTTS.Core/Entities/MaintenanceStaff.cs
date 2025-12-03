namespace YBTTS.Core.Entities;

public class MaintenanceStaff
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty; // "Elektrik", "Tesisat", "Genel" vb.
    public int CompletedTasksCount { get; set; } = 0;
    public double AverageRating { get; set; } = 0.0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Request> AssignedRequests { get; set; } = new List<Request>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
