using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Infrastructure.Data;

public class StudentAuthService
{
    private readonly YbttsDbContext _context;

    public StudentAuthService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> LoginAsync(string studentNumber, string password)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber && s.Password == password);
    }
}
