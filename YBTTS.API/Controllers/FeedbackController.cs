using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    /// <summary>
    /// Geri bildirim oluşturma
    /// POST /api/feedback
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Geçersiz veriler" });

            var feedback = await _feedbackService.CreateAsync(dto.RequestId, dto.StudentId, dto.Rating, dto.Comment);

            return Created(string.Empty, new
            {
                success = true,
                message = "Geri bildiriminiz başarıyla kaydedildi. +5 puan kazandınız!",
                data = new
                {
                    id = feedback.Id,
                    requestId = feedback.RequestId,
                    studentId = feedback.StudentId,
                    rating = feedback.Rating,
                    comment = feedback.Comment,
                    createdAt = feedback.CreatedAt
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Talebe ait geri bildirimleri getir
    /// GET /api/feedback/request/{requestId}
    /// </summary>
    [HttpGet("request/{requestId}")]
    public async Task<IActionResult> GetByRequest(int requestId)
    {
        try
        {
            var feedbacks = await _feedbackService.GetByRequestAsync(requestId);

            var data = feedbacks.Select(f => new
            {
                id = f.Id,
                requestId = f.RequestId,
                studentId = f.StudentId,
                studentName = f.Student?.FullName,
                rating = f.Rating,
                comment = f.Comment,
                createdAt = f.CreatedAt
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Geri bildirimler başarıyla getirildi",
                data
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }
}

public class CreateFeedbackDto
{
    public int RequestId { get; set; }
    public int StudentId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}