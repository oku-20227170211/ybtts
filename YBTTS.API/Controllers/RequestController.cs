using Microsoft.AspNetCore.Mvc;
using YBTTS.Core.Enums;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    /// <summary>
    /// 1️⃣ Öğrenci talep oluşturma
    /// POST /api/requests
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Geçersiz veriler" });

            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Description))
                return BadRequest(new { success = false, message = "Başlık ve açıklama gereklidir" });

            var request = await _requestService.CreateAsync(dto.StudentId, dto.Title, dto.Description);

            var responseData = new
            {
                id = request.Id,
                studentId = request.StudentId,
                title = request.Title,
                description = request.Description,
                status = request.Status.ToString(),
                createdAt = request.CreatedAt,
                completedAt = request.CompletedAt,
                satisfactionScore = request.SatisfactionScore
            };

            return Created(string.Empty, new
            {
                success = true,
                message = "Talep başarıyla oluşturuldu",
                data = responseData
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
    /// 2️⃣ Öğrencinin kendi taleplerini listeleme
    /// GET /api/requests/student/{studentId}
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentRequests(int studentId)
    {
        try
        {
            var requests = await _requestService.GetByStudentAsync(studentId);

            var responseData = requests.Select(r => new
            {
                id = r.Id,
                studentId = r.StudentId,
                title = r.Title,
                description = r.Description,
                status = r.Status.ToString(),
                createdAt = r.CreatedAt,
                completedAt = r.CompletedAt,
                satisfactionScore = r.SatisfactionScore
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Talepler başarıyla getirildi",
                data = responseData
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// 3️⃣ Admin tüm talepleri listeleme
    /// GET /api/requests
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRequests()
    {
        try
        {
            var requests = await _requestService.GetAllAsync();

            var responseData = requests.Select(r => new
            {
                id = r.Id,
                studentId = r.StudentId,
                title = r.Title,
                description = r.Description,
                status = r.Status.ToString(),
                createdAt = r.CreatedAt,
                completedAt = r.CompletedAt,
                satisfactionScore = r.SatisfactionScore,
                student = r.Student != null ? new
                {
                    id = r.Student.Id,
                    fullName = r.Student.FullName,
                    studentNumber = r.Student.StudentNumber
                } : null
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Tüm talepler başarıyla getirildi",
                data = responseData
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// 4️⃣ Admin talep durum güncelleme
    /// PUT /api/requests/{id}
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] UpdateRequestStatusDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Geçersiz veriler" });

            var request = await _requestService.UpdateStatusAsync(id, dto.Status);

            if (request == null)
                return NotFound(new { success = false, message = $"Talep ID {id} bulunamadı" });

            var responseData = new
            {
                id = request.Id,
                studentId = request.StudentId,
                title = request.Title,
                description = request.Description,
                status = request.Status.ToString(),
                createdAt = request.CreatedAt,
                completedAt = request.CompletedAt,
                satisfactionScore = request.SatisfactionScore
            };

            return Ok(new
            {
                success = true,
                message = "Talep durumu başarıyla güncellendi",
                data = responseData
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// 5️⃣ Tamamlanan talep için memnuniyet puanı ekleme
    /// POST /api/requests/{id}/satisfaction
    /// </summary>
    [HttpPost("{id}/satisfaction")]
    public async Task<IActionResult> SubmitSatisfaction(int id, [FromBody] SubmitSatisfactionDto dto)
    {
        try
        {
            if (dto.SatisfactionScore is < 1 or > 5)
                return BadRequest(new { success = false, message = "Memnuniyet puanı 1 ile 5 arasında olmalıdır" });

            var request = await _requestService.SubmitSatisfactionAsync(id, dto.SatisfactionScore);

            if (request == null)
                return NotFound(new { success = false, message = $"Talep ID {id} bulunamadı" });

            var responseData = new
            {
                id = request.Id,
                studentId = request.StudentId,
                title = request.Title,
                description = request.Description,
                status = request.Status.ToString(),
                createdAt = request.CreatedAt,
                completedAt = request.CompletedAt,
                satisfactionScore = request.SatisfactionScore
            };

            return Ok(new
            {
                success = true,
                message = "Memnuniyet puanı kaydedildi",
                data = responseData
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
}

/// <summary>
/// Talep oluşturma DTO
/// </summary>
public class CreateRequestDto
{
    public int StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Talep durumu güncelleme DTO
/// </summary>
public class UpdateRequestStatusDto
{
    public RequestStatus Status { get; set; }
}

/// <summary>
/// Memnuniyet puanı girme DTO
/// </summary>
public class SubmitSatisfactionDto
{
    public int SatisfactionScore { get; set; }
}
