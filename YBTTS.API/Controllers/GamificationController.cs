using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/gamification")]
public class GamificationController : ControllerBase
{
    private readonly IGamificationService _gamificationService;

    public GamificationController(IGamificationService gamificationService)
    {
        _gamificationService = gamificationService;
    }

    /// <summary>
    /// Liderlik tablosunu getir
    /// GET /api/gamification/leaderboard
    /// </summary>
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int topN = 10)
    {
        try
        {
            var leaderboard = await _gamificationService.GetLeaderboardAsync(topN);

            return Ok(new
            {
                success = true,
                message = "Liderlik tablosu başarıyla getirildi",
                data = leaderboard
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Öğrenci istatistiklerini getir
    /// GET /api/gamification/student/{studentId}/stats
    /// </summary>
    [HttpGet("student/{studentId}/stats")]
    public async Task<IActionResult> GetStudentStats(int studentId)
    {
        try
        {
            var stats = await _gamificationService.GetStudentStatsAsync(studentId);

            return Ok(new
            {
                success = true,
                message = "Öğrenci istatistikleri başarıyla getirildi",
                data = stats
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Manuel puan ekleme (Admin)
    /// POST /api/gamification/add-points
    /// </summary>
    [HttpPost("add-points")]
    public async Task<IActionResult> AddPoints([FromBody] AddPointsDto dto)
    {
        try
        {
            var student = await _gamificationService.AddPointsAsync(dto.StudentId, dto.Points, dto.Reason);

            return Ok(new
            {
                success = true,
                message = "Puan başarıyla eklendi",
                data = new
                {
                    studentId = student.Id,
                    fullName = student.FullName,
                    score = student.Score,
                    level = student.Level
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }
}

public class AddPointsDto
{
    public int StudentId { get; set; }
    public int Points { get; set; }
    public string Reason { get; set; } = string.Empty;
}