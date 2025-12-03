using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var student = await _studentService.CreateStudentAsync(request.FullName, request.StudentNumber, request.Password);
        return CreatedAtAction(nameof(GetAllStudents), new { id = student.Id }, student);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var result = await _studentService.DeleteStudentAsync(id);

        if (!result)
        {
            return NotFound(new { message = "Öğrenci bulunamadı." });
        }

        return Ok(new { message = "Öğrenci başarıyla silindi." });
    }
}

public class CreateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
