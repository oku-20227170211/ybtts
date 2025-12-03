using YBTTS.Core.Entities;

namespace YBTTS.Infrastructure.Services;

public interface IGamificationService
{
    Task<Student> AddPointsAsync(int studentId, int points, string reason);
    Task<Student> CalculateLevelAsync(int studentId);
    Task<List<Badge>> CheckAndAwardBadgesAsync(int studentId);
    Task<List<LeaderboardEntry>> GetLeaderboardAsync(int topN = 10);
    Task<StudentStats> GetStudentStatsAsync(int studentId);
}

public class LeaderboardEntry
{
    public int Rank { get; set; }
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Level { get; set; }
    public int BadgeCount { get; set; }
}

public class StudentStats
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Level { get; set; }
    public int NextLevelPoints { get; set; }
    public int TotalRequests { get; set; }
    public int CompletedRequests { get; set; }
    public int TotalFeedbacks { get; set; }
    public List<Badge> Badges { get; set; } = new();
}