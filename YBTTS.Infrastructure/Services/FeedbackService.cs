using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class FeedbackService : IFeedbackService
{
    private readonly YbttsDbContext _context;
    private readonly IGamificationService _gamificationService;

    public FeedbackService(YbttsDbContext context, IGamificationService gamificationService)
    {
        _context = context;
        _gamificationService = gamificationService;
    }

    public async Task<Feedback> CreateAsync(int requestId, int studentId, int rating, string comment)
    {
        // Talebin varlığını ve tamamlanmış olduğunu kontrol et
        var request = await _context.Requests
            .Include(r => r.AssignedToStaff)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null)
            throw new InvalidOperationException($"Talep ID {requestId} bulunamadı.");

        if (request.Status != RequestStatus.Completed)
            throw new InvalidOperationException("Sadece tamamlanmış talepler için geri bildirim verilebilir.");

        if (request.StudentId != studentId)
            throw new InvalidOperationException("Sadece kendi taleplerinize geri bildirim verebilirsiniz.");

        // Daha önce geri bildirim verilmiş mi kontrol et
        var existingFeedback = await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.RequestId == requestId && f.StudentId == studentId);

        if (existingFeedback != null)
            throw new InvalidOperationException("Bu talep için zaten geri bildirim verdiniz.");

        // Rating 1-5 arasında olmalı
        if (rating < 1 || rating > 5)
            throw new InvalidOperationException("Değerlendirme 1 ile 5 arasında olmalıdır.");

        var feedback = new Feedback
        {
            RequestId = requestId,
            StudentId = studentId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        // Öğrenciye geri bildirim için puan ekle
        await _gamificationService.AddPointsAsync(studentId, 5, "Geri bildirim verme");

        // Bakım personelinin ortalama değerlendirmesini güncelle
        if (request.AssignedToStaffId.HasValue)
        {
            var averageRating = await GetAverageRatingForStaffAsync(request.AssignedToStaffId.Value);
            var staff = await _context.MaintenanceStaffs.FindAsync(request.AssignedToStaffId.Value);
            if (staff != null)
            {
                staff.AverageRating = averageRating;
                await _context.SaveChangesAsync();
            }
        }

        return feedback;
    }

    public async Task<List<Feedback>> GetByRequestAsync(int requestId)
    {
        return await _context.Feedbacks
            .Include(f => f.Student)
            .Where(f => f.RequestId == requestId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingForStaffAsync(int staffId)
    {
        var ratings = await _context.Feedbacks
            .Where(f => f.Request.AssignedToStaffId == staffId)
            .Select(f => f.Rating)
            .ToListAsync();

        if (!ratings.Any())
            return 0.0;

        return Math.Round(ratings.Average(), 2);
    }
}