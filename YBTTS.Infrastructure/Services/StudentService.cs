using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Infrastructure.Data;


namespace YBTTS.Infrastructure.Services;

public class StudentService
{
    private readonly YbttsDbContext _context;

    public StudentService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<Student> CreateStudentAsync(string fullName, string studentNumber, string password)
    {
        var student = new Student
        {
            FullName = fullName,
            StudentNumber = studentNumber,
            Password = password,
            Points = 0,
            Level = 1
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _context.Students
            .Include(s => s.Requests)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Requests)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Student>> GetLeaderboardAsync(int take = 10)
    {
        return await _context.Students
            .Include(s => s.Requests)
            .AsNoTracking()
            .OrderByDescending(s => s.Points)
            .ThenBy(s => s.FullName)
            .Take(take)
            .ToListAsync();
    }

    public void ApplyPoints(Student student, int points)
    {
        student.Points += points;
        student.Level = CalculateLevel(student.Points);
    }

    private static int CalculateLevel(int points)
    {
        const int pointsPerLevel = 100;
        return Math.Max(1, (points / pointsPerLevel) + 1);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }
}
