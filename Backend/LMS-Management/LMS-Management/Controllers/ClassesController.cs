using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Management.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly IClassService _classService;
    private readonly IRegistrationService _registrationService;

    public ClassesController(IClassService classService, IRegistrationService registrationService)
    {
        _classService = classService;
        _registrationService = registrationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassDto dto)
    {
        var result = await _classService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByDay), new { }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByDay([FromQuery] string? day)
    {
        var result = await _classService.GetByDayAsync(day);
        return Ok(result);
    }

    [HttpPost("{classId}/register")]
    public async Task<IActionResult> Register(string classId, [FromBody] RegisterStudentDto dto)
    {
        try
        {
            var result = await _registrationService.RegisterAsync(classId, dto);
            return CreatedAtAction(nameof(Register), new { classId }, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}
