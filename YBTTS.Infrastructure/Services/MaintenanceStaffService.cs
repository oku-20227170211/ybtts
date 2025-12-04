using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class MaintenanceStaffService : IMaintenanceStaffService
{
    private readonly YbttsDbContext _context;

    public MaintenanceStaffService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceStaff?> LoginAsync(string username, string password)
    {
        return await _context.MaintenanceStaffs
            .FirstOrDefaultAsync(m => m.Username == username && m.Password == password);
    }

    public async Task<List<MaintenanceStaff>> GetAllAsync()
    {
        return await _context.MaintenanceStaffs
            .OrderBy(m => m.FullName)
            .ToListAsync();
    }

    public async Task<MaintenanceStaff?> GetByIdAsync(int id)
    {
        return await _context.MaintenanceStaffs
            .Include(m => m.AssignedRequests)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Request>> GetAssignedRequestsAsync(int staffId)
    {
        return await _context.Requests
            .Include(r => r.Student)
            .Where(r => r.AssignedToStaffId == staffId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<MaintenanceStaff> CreateAsync(string fullName, string username, string password, string email, string specialization)
    {
        // Kullanıcı adı kontrolu
        var exists = await _context.MaintenanceStaffs.AnyAsync(m => m.Username == username);
        if (exists)
            throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");

        var staff = new MaintenanceStaff
        {
            FullName = fullName,
            Username = username,
            Password = password,
            Email = email,
            Specialization = specialization,
            CreatedAt = DateTime.UtcNow
        };

        _context.MaintenanceStaffs.Add(staff);
        await _context.SaveChangesAsync();

        return staff;
    }

    public async Task<MaintenanceStaff?> UpdateAsync(int id, string? fullName, string? specialization)
    {
        var staff = await _context.MaintenanceStaffs.FindAsync(id);
        if (staff == null)
            return null;

        if (!string.IsNullOrWhiteSpace(fullName))
            staff.FullName = fullName;

        if (!string.IsNullOrWhiteSpace(specialization))
            staff.Specialization = specialization;

        await _context.SaveChangesAsync();
        return staff;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var staff = await _context.MaintenanceStaffs.FindAsync(id);
        if (staff == null)
            return false;

        _context.MaintenanceStaffs.Remove(staff);
        await _context.SaveChangesAsync();
        return true;
    }
}