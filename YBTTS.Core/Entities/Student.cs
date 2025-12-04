namespace YBTTS.Core.Entities;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoomNo { get; set; } = string.Empty;
    
    // Gamification fields
    public int Score { get; set; } = 0;
    public int Level { get; set; } = 1;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Request> Requests { get; set; } = new List<Request>();
    public ICollection<StudentBadge> StudentBadges { get; set; } = new List<StudentBadge>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}