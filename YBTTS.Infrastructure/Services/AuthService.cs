using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;
using YBTTS.Infrastructure.Data;

namespace YBTTS.Infrastructure.Services;

public class AuthService
{
    private readonly YbttsDbContext _context;

    public AuthService(YbttsDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, User? User, string Message)> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return (false, null, "Kullanıcı adı ve şifre boş olamaz.");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return (false, null, "Kullanıcı bulunamadı.");
        }

        if (user.Password != password)
        {
            return (false, null, "Şifre yanlış.");
        }

        return (true, user, "Başarıyla giriş yapıldı.");
    }

    public async Task<User> CreateUserAsync(string username, string password, string role = "Admin")
    {
        var user = new User
        {
            Username = username,
            Password = password,
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }
}
