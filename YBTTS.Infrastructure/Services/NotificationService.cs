using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly YbttsDbContext _context;

    public NotificationService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> CreateForStudentAsync(int studentId, string message, string type)
    {
        var notification = new Notification
        {
            StudentId = studentId,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return notification;
    }

    public async Task<Notification> CreateForStaffAsync(int staffId, string message, string type)
    {
        var notification = new Notification
        {
            MaintenanceStaffId = staffId,
            Message = message,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return notification;
    }

    public async Task<List<Notification>> GetStudentNotificationsAsync(int studentId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.StudentId == studentId);

        if (unreadOnly)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Notification>> GetStaffNotificationsAsync(int staffId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.MaintenanceStaffId == staffId);

        if (unreadOnly)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null)
            return false;

        notification.IsRead = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetUnreadCountForStudentAsync(int studentId)
    {
        return await _context.Notifications
            .CountAsync(n => n.StudentId == studentId && !n.IsRead);
    }

    public async Task<int> GetUnreadCountForStaffAsync(int staffId)
    {
        return await _context.Notifications
            .CountAsync(n => n.MaintenanceStaffId == staffId && !n.IsRead);
    }
}