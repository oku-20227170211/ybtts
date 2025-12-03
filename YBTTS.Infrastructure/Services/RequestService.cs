using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class RequestService : IRequestService
{
    private readonly YbttsDbContext _context;
    private const int CreateRequestPoints = 10;
    private const int SatisfactionPoints = 5;

    public RequestService(YbttsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Öğrenci tarafından yeni talep oluşturma
    /// </summary>
    public async Task<Request> CreateAsync(int studentId, string title, string description)
    {
        // Öğrencinin var olup olmadığını kontrol et
        var student = await _context.Students.FirstOrDefaultAsync(x=>x.Id==studentId);
        if (student == null)
            throw new InvalidOperationException($"Öğrenci ID {studentId} bulunamadı.");

        var request = new Request
        {
            StudentId = studentId,
            Title = title,
            Description = description,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Requests.Add(request);
        AwardPoints(student, CreateRequestPoints);
        await _context.SaveChangesAsync();

        return request;
    }

    /// <summary>
    /// Öğrencinin kendi taleplerini getirme
    /// </summary>
    public async Task<List<Request>> GetByStudentAsync(int studentId)
    {
        return await _context.Requests
            .Where(r => r.StudentId == studentId)
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Tüm talepleri getirme (Admin)
    /// </summary>
    public async Task<List<Request>> GetAllAsync()
    {
        return await _context.Requests
            .Include(r => r.Student)
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Talep durumu güncelleme
    /// </summary>
    public async Task<Request?> UpdateStatusAsync(int requestId, RequestStatus status)
    {
        var request = await _context.Requests.FindAsync(requestId);
        if (request == null)
            return null;

        request.Status = status;

        if (status == RequestStatus.Completed)
        {
            request.CompletedAt = DateTime.UtcNow;
        }
        else
        {
            request.CompletedAt = null;
            request.SatisfactionScore = null;
        }

        await _context.SaveChangesAsync();

        return request;
    }

    /// <summary>
    /// Tamamlanan talep için memnuniyet puanı ekleme
    /// </summary>
    public async Task<Request?> SubmitSatisfactionAsync(int requestId, int satisfactionScore)
    {
        var request = await _context.Requests.FindAsync(requestId);
        if (request == null)
            return null;

        if (request.Status != RequestStatus.Completed)
            throw new InvalidOperationException("Memnuniyet puanı yalnızca tamamlanan talepler için girilebilir.");

        if (request.CompletedAt == null)
            throw new InvalidOperationException("Talebin tamamlanma tarihi bulunamadı. Lütfen yöneticiyle iletişime geçin.");

        if (request.SatisfactionScore.HasValue)
            throw new InvalidOperationException("Bu talep için memnuniyet puanı daha önce girilmiş.");

        request.SatisfactionScore = satisfactionScore;
        var student = await _context.Students.FindAsync(request.StudentId);
        if (student != null)
            AwardPoints(student, SatisfactionPoints);
        await _context.SaveChangesAsync();

        return request;
    }

    private static void AwardPoints(Student student, int points)
    {
        const int pointsPerLevel = 100;
        student.Points += points;
        student.Level = Math.Max(1, (student.Points / pointsPerLevel) + 1);
    }
}
