using YBTTS.Core.Entities;
using YBTTS.Core.Enums;

namespace YBTTS.Infrastructure.Services;

public interface IRequestService
{
    /// <summary>
    /// Öğrenci tarafından yeni talep oluşturma
    /// </summary>
    Task<Request> CreateAsync(int studentId, string title, string description);

    /// <summary>
    /// Öğrencinin kendi taleplerini getirme
    /// </summary>
    Task<List<Request>> GetByStudentAsync(int studentId);

    /// <summary>
    /// Tüm talepleri getirme (Admin)
    /// </summary>
    Task<List<Request>> GetAllAsync();

    /// <summary>
    /// Talep durumu güncelleme
    /// </summary>
    Task<Request?> UpdateStatusAsync(int requestId, RequestStatus status);

    /// <summary>
    /// Tamamlanan talep için memnuniyet puanı ekleme
    /// </summary>
    Task<Request?> SubmitSatisfactionAsync(int requestId, int satisfactionScore);
}
