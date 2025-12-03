using YBTTS.Core.Entities;
using YBTTS.Core.Enums;

namespace YBTTS.Infrastructure.Services;

public interface IRequestService
{
    /// <summary>
    /// Öğrenci tarafından yeni talep oluşturma
    /// </summary>
    Task<Request> CreateAsync(int studentId, string title, string description, string roomNo="" );

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
    /// Talebi bakım personeline atama
    /// </summary>
    Task<Request?> AssignToStaffAsync(int requestId, int staffId);

    /// <summary>
    /// Talebi tamamlama
    /// </summary>
    Task<Request?> CompleteRequestAsync(int requestId, int staffId);

    /// <summary>
    /// Talebi işleme alma
    /// </summary>
    Task<Request?> StartProgressAsync(int requestId, int staffId);
}