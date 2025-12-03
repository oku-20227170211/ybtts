using YBTTS.Core.Entities;

namespace YBTTS.Infrastructure.Services;

public interface IMaintenanceStaffService
{
    Task<MaintenanceStaff?> LoginAsync(string username, string password);
    Task<List<MaintenanceStaff>> GetAllAsync();
    Task<MaintenanceStaff?> GetByIdAsync(int id);
    Task<List<Request>> GetAssignedRequestsAsync(int staffId);
    Task<MaintenanceStaff> CreateAsync(string fullName, string username, string password, string email, string specialization);
    Task<MaintenanceStaff?> UpdateAsync(int id, string? fullName, string? specialization);
    Task<bool> DeleteAsync(int id);
}
