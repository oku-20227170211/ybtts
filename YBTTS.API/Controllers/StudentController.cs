using Microsoft.AspNetCore.Mvc;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, MapStudent(student));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        var response = students.Select(MapStudent).ToList();
        return Ok(new { success = true, data = response });
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int take = 10)
    {
        var students = await _studentService.GetLeaderboardAsync(take);
        var response = students.Select(s => MapStudent(s, includeRequestStats: true)).ToList();
        return Ok(new { success = true, data = response });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound(new { success = false, message = "Öğrenci bulunamadı." });
        }

        return Ok(new { success = true, data = MapStudent(student, includeRequestStats: true) });
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

    private static object MapStudent(Core.Entities.Student student, bool includeRequestStats = false)
    {
        var totalRequests = includeRequestStats ? student.Requests.Count : student.Requests?.Count ?? 0;
        var completedRequests = includeRequestStats
            ? student.Requests.Count(r => r.Status == RequestStatus.Completed)
            : 0;
        var satisfactionScores = includeRequestStats
            ? student.Requests.Where(r => r.SatisfactionScore.HasValue).Select(r => r.SatisfactionScore!.Value).ToList()
            : new List<int>();

        double? averageSatisfaction = null;
        if (satisfactionScores.Any())
        {
            averageSatisfaction = Math.Round(satisfactionScores.Average(), 1);
        }

        return new
        {
            id = student.Id,
            fullName = student.FullName,
            studentNumber = student.StudentNumber,
            points = student.Points,
            level = student.Level,
            totalRequests = totalRequests,
            completedRequests = completedRequests,
            averageSatisfaction = averageSatisfaction
        };
    }
}

public class CreateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
