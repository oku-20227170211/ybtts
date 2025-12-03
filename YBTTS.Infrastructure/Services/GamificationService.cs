using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class GamificationService : IGamificationService
{
    private readonly YbttsDbContext _context;

    public GamificationService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<Student> AddPointsAsync(int studentId, int points, string reason)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
            throw new InvalidOperationException($"Öğrenci ID {studentId} bulunamadı.");

        student.Score += points;
        await _context.SaveChangesAsync();

        // Seviye hesapla
        await CalculateLevelAsync(studentId);

        // Rozet kontrolü
        await CheckAndAwardBadgesAsync(studentId);

        return student;
    }

    public async Task<Student> CalculateLevelAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
            throw new InvalidOperationException($"Öğrenci ID {studentId} bulunamadı.");

        int newLevel = CalculateLevel(student.Score);
        int oldLevel = student.Level;

        if (newLevel > oldLevel)
        {
            student.Level = newLevel;
            await _context.SaveChangesAsync();

            // Seviye atlama bildirimi
            var notification = new Notification
            {
                StudentId = studentId,
                Message = $"Tebrikler! {newLevel}. seviyeye ulaştınız! 🎉",
                Type = "Level"
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        return student;
    }

    public async Task<List<Badge>> CheckAndAwardBadgesAsync(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.StudentBadges)
            .ThenInclude(sb => sb.Badge)
            .Include(s => s.Requests)
            .Include(s => s.Feedbacks)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            throw new InvalidOperationException($"Öğrenci ID {studentId} bulunamadı.");

        var earnedBadgeIds = student.StudentBadges.Select(sb => sb.BadgeId).ToList();
        var allBadges = await _context.Badges.ToListAsync();
        var newlyEarnedBadges = new List<Badge>();

        foreach (var badge in allBadges)
        {
            if (earnedBadgeIds.Contains(badge.Id))
                continue;

            bool shouldAward = false;

            // Badge kriterlerini kontrol et
            if (badge.Name == "İlk Adım" && student.Requests.Count >= 1)
                shouldAward = true;
            else if (badge.Name == "Aktif Üye" && student.Requests.Count >= 5)
                shouldAward = true;
            else if (badge.Name == "Geri Bildirim Ustası" && student.Feedbacks.Count >= 10)
                shouldAward = true;
            else if (badge.Name == "Yıldız Öğrenci" && student.Level >= 3)
                shouldAward = true;
            else if (badge.Name == "Yurt Kahramanı" && student.Level >= 5)
                shouldAward = true;

            if (shouldAward)
            {
                var studentBadge = new StudentBadge
                {
                    StudentId = studentId,
                    BadgeId = badge.Id,
                    EarnedAt = DateTime.UtcNow
                };
                _context.StudentBadges.Add(studentBadge);
                newlyEarnedBadges.Add(badge);

                // Rozet kazanma bildirimi
                var notification = new Notification
                {
                    StudentId = studentId,
                    Message = $"Tebrikler! '{badge.Name}' rozetini kazandınız! {badge.IconUrl}",
                    Type = "Badge"
                };
                _context.Notifications.Add(notification);
            }
        }

        if (newlyEarnedBadges.Any())
            await _context.SaveChangesAsync();

        return newlyEarnedBadges;
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboardAsync(int topN = 10)
    {
        var students = await _context.Students
            .Include(s => s.StudentBadges)
            .OrderByDescending(s => s.Score)
            .ThenByDescending(s => s.Level)
            .Take(topN)
            .ToListAsync();

        var leaderboard = students.Select((student, index) => new LeaderboardEntry
        {
            Rank = index + 1,
            StudentId = student.Id,
            FullName = student.FullName,
            StudentNumber = student.StudentNumber,
            Score = student.Score,
            Level = student.Level,
            BadgeCount = student.StudentBadges.Count
        }).ToList();

        return leaderboard;
    }

    public async Task<StudentStats> GetStudentStatsAsync(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Requests)
            .Include(s => s.Feedbacks)
            .Include(s => s.StudentBadges)
            .ThenInclude(sb => sb.Badge)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            throw new InvalidOperationException($"Öğrenci ID {studentId} bulunamadı.");

        var completedRequests = student.Requests.Count(r => r.Status == RequestStatus.Completed);
        var nextLevelPoints = GetPointsForNextLevel(student.Level);

        return new StudentStats
        {
            StudentId = student.Id,
            FullName = student.FullName,
            Score = student.Score,
            Level = student.Level,
            NextLevelPoints = nextLevelPoints,
            TotalRequests = student.Requests.Count,
            CompletedRequests = completedRequests,
            TotalFeedbacks = student.Feedbacks.Count,
            Badges = student.StudentBadges.Select(sb => sb.Badge).ToList()
        };
    }

    private int CalculateLevel(int score)
    {
        if (score >= 500) return 5;
        if (score >= 300) return 4;
        if (score >= 150) return 3;
        if (score >= 50) return 2;
        return 1;
    }

    private int GetPointsForNextLevel(int currentLevel)
    {
        return currentLevel switch
        {
            1 => 50,
            2 => 150,
            3 => 300,
            4 => 500,
            _ => 0 // Max level
        };
    }
}