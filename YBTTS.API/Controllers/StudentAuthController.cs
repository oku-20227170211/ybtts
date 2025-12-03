using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

[ApiController]
[Route("api/student-auth")]
public class StudentAuthController : ControllerBase
{
    private readonly StudentAuthService _auth;

    public StudentAuthController(StudentAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] StudentLoginDto dto)
    {
        var student = await _auth.LoginAsync(dto.StudentNumber, dto.Password);

        if (student == null)
            return Unauthorized(new { success = false, message = "Öğrenci numarası veya şifre yanlış." });

        return Ok(new
        {
            success = true,
            student = new
            {
                id = student.Id,
                fullName = student.FullName,
                studentNumber = student.StudentNumber,
                points = student.Points,
                level = student.Level
            }
        });
    }
}

public class StudentLoginDto
{
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
