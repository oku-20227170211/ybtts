using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class RequestService : IRequestService
{
    private readonly YbttsDbContext _context;
    private readonly IGamificationService _gamificationService;
    private readonly INotificationService _notificationService;

    public RequestService(YbttsDbContext context, IGamificationService gamificationService, INotificationService notificationService)
    {
        _context = context;
        _gamificationService = gamificationService;
        _notificationService = notificationService;
    }
    /// <summary>
    /// Öğrenci tarafından yeni talep oluşturma
    /// </summary>
        public async Task<Request> CreateAsync(int studentId, string title, string description, string roomNo="")
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
            RoomNo = string.IsNullOrWhiteSpace(roomNo) ? student.RoomNo : roomNo,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Requests.Add(request);
        await _context.SaveChangesAsync();

        // Öğrenciye puan ekle
        await _gamificationService.AddPointsAsync(studentId, 10, "Talep oluşturma");

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
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<bool> DeleteAsync(int id)
{
    var request = await _context.Requests.FindAsync(id);

    if (request == null)
        return false;

    _context.Requests.Remove(request);
    await _context.SaveChangesAsync();

    return true;
}
public async Task<bool> SetPendingAsync(int id)
{
    var request = await _context.Requests.FindAsync(id);

    if (request == null)
        return false;

    request.Status = RequestStatus.Pending;
    await _context.SaveChangesAsync();

    return true;
}

    public Task<Request?> AssignToStaffAsync(int requestId, int staffId)
    {
        throw new NotImplementedException();
    }

    public Task<Request?> CompleteRequestAsync(int requestId, int staffId)
    {
        throw new NotImplementedException();
    }

    public Task<Request?> StartProgressAsync(int requestId, int staffId)
    {
        throw new NotImplementedException();
    }
}
