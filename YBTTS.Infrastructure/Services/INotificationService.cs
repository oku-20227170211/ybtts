using YBTTS.Core.Entities;

namespace YBTTS.Infrastructure.Services;

public interface INotificationService
{
    Task<Notification> CreateForStudentAsync(int studentId, string message, string type);
    Task<Notification> CreateForStaffAsync(int staffId, string message, string type);
    Task<List<Notification>> GetStudentNotificationsAsync(int studentId, bool unreadOnly = false);
    Task<List<Notification>> GetStaffNotificationsAsync(int staffId, bool unreadOnly = false);
    Task<bool> MarkAsReadAsync(int notificationId);
    Task<int> GetUnreadCountForStudentAsync(int studentId);
    Task<int> GetUnreadCountForStaffAsync(int staffId);
}
