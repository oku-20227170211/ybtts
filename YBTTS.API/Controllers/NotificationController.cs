using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Öğrenci bildirimlerini getir
    /// GET /api/notifications/student/{studentId}
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentNotifications(int studentId, [FromQuery] bool unreadOnly = false)
    {
        try
        {
            var notifications = await _notificationService.GetStudentNotificationsAsync(studentId, unreadOnly);

            var data = notifications.Select(n => new
            {
                id = n.Id,
                message = n.Message,
                type = n.Type,
                isRead = n.IsRead,
                createdAt = n.CreatedAt
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Bildirimler başarıyla getirildi",
                data
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Bakım personeli bildirimlerini getir
    /// GET /api/notifications/staff/{staffId}
    /// </summary>
    [HttpGet("staff/{staffId}")]
    public async Task<IActionResult> GetStaffNotifications(int staffId, [FromQuery] bool unreadOnly = false)
    {
        try
        {
            var notifications = await _notificationService.GetStaffNotificationsAsync(staffId, unreadOnly);

            var data = notifications.Select(n => new
            {
                id = n.Id,
                message = n.Message,
                type = n.Type,
                isRead = n.IsRead,
                createdAt = n.CreatedAt
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Bildirimler başarıyla getirildi",
                data
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Bildirimi okundu olarak işaretle
    /// PUT /api/notifications/{id}/read
    /// </summary>
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var success = await _notificationService.MarkAsReadAsync(id);

            if (!success)
                return NotFound(new { success = false, message = $"Bildirim ID {id} bulunamadı" });

            return Ok(new
            {
                success = true,
                message = "Bildirim okundu olarak işaretlendi"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Öğrenci için okunmamış bildirim sayısı
    /// GET /api/notifications/student/{studentId}/unread-count
    /// </summary>
    [HttpGet("student/{studentId}/unread-count")]
    public async Task<IActionResult> GetUnreadCountForStudent(int studentId)
    {
        try
        {
            var count = await _notificationService.GetUnreadCountForStudentAsync(studentId);

            return Ok(new
            {
                success = true,
                count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }
}
