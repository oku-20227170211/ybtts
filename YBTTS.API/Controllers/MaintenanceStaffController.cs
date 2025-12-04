using Microsoft.AspNetCore.Mvc;
using YBTTS.Infrastructure.Services;

namespace YBTTS.API.Controllers;

[ApiController]
[Route("api/staff")]
public class MaintenanceStaffController : ControllerBase
{
    private readonly IMaintenanceStaffService _staffService;
    private readonly IRequestService _requestService;

    public MaintenanceStaffController(IMaintenanceStaffService staffService, IRequestService requestService)
    {
        _staffService = staffService;
        _requestService = requestService;
    }

    /// <summary>
    /// Bakım personeli girişi
    /// POST /api/staff/login
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] StaffLoginDto dto)
    {
        var staff = await _staffService.LoginAsync(dto.Username, dto.Password);

        if (staff == null)
            return Unauthorized(new { success = false, message = "Kullanıcı adı veya şifre yanlış." });

        return Ok(new
        {
            success = true,
            staff = new
            {
                id = staff.Id,
                fullName = staff.FullName,
                username = staff.Username,
                email = staff.Email,
                specialization = staff.Specialization,
                completedTasksCount = staff.CompletedTasksCount,
                averageRating = staff.AverageRating
            }
        });
    }

    /// <summary>
    /// Tüm bakım personelini listele (Admin)
    /// GET /api/staff
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var staffList = await _staffService.GetAllAsync();

            var data = staffList.Select(s => new
            {
                id = s.Id,
                fullName = s.FullName,
                username = s.Username,
                email = s.Email,
                specialization = s.Specialization,
                completedTasksCount = s.CompletedTasksCount,
                averageRating = s.AverageRating
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Bakım personeli listesi başarıyla getirildi",
                data
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Bakım personeline atanan talepleri getir
    /// GET /api/staff/{staffId}/requests
    /// </summary>
    [HttpGet("{staffId}/requests")]
    public async Task<IActionResult> GetAssignedRequests(int staffId)
    {
        try
        {
            var requests = await _staffService.GetAssignedRequestsAsync(staffId);

            var data = requests.Select(r => new
            {
                id = r.Id,
                studentId = r.StudentId,
                studentName = r.Student?.FullName,
                studentNumber = r.Student?.StudentNumber,
                title = r.Title,
                description = r.Description,
                roomNo = r.RoomNo,
                status = r.Status.ToString(),
                createdAt = r.CreatedAt,
                completedAt = r.CompletedAt
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "Atanan talepler başarıyla getirildi",
                data
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Sunucu hatası", error = ex.Message });
        }
    }

    /// <summary>
    /// Talebi işleme alma
    /// PUT /api/staff/requests/{requestId}/start
    /// </summary>
    [HttpPut("requests/{requestId}/start")]
    public async Task<IActionResult> StartProgress(int requestId, [FromBody] StaffActionDto dto)
    {
        try
        {
            var request = await _requestService.StartProgressAsync(requestId, dto.StaffId);

            if (request == null)
                return NotFound(new { success = false, message = $"Talep ID {requestId} bulunamadı" });

            return Ok(new
            {
                success = true,
                message = "Talep işleme alındı",
                data = new
                {
                    id = request.Id,
                    status = request.Status.ToString()
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
    /// Talebi tamamlama
    /// PUT /api/staff/requests/{requestId}/complete
    /// </summary>
    [HttpPut("requests/{requestId}/complete")]
    public async Task<IActionResult> CompleteRequest(int requestId, [FromBody] StaffActionDto dto)
    {
        try
        {
            var request = await _requestService.CompleteRequestAsync(requestId, dto.StaffId);

            if (request == null)
                return NotFound(new { success = false, message = $"Talep ID {requestId} bulunamadı" });

            return Ok(new
            {
                success = true,
                message = "Talep başarıyla tamamlandı",
                data = new
                {
                    id = request.Id,
                    status = request.Status.ToString(),
                    completedAt = request.CompletedAt
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
    /// Yeni bakım personeli ekleme (Admin)
    /// POST /api/staff
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStaffDto dto)
    {
        try
        {
            var staff = await _staffService.CreateAsync(
                dto.FullName,
                dto.Username,
                dto.Password,
                dto.Email,
                dto.Specialization
            );

            return Created(string.Empty, new
            {
                success = true,
                message = "Bakım personeli başarıyla eklendi",
                data = new
                {
                    id = staff.Id,
                    fullName = staff.FullName,
                    username = staff.Username,
                    email = staff.Email,
                    specialization = staff.Specialization
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
}

public class StaffLoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class StaffActionDto
{
    public int StaffId { get; set; }
}

public class CreateStaffDto
{
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}