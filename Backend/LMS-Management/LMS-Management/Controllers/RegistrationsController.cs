using LMS_Management.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Management.Controllers;

[ApiController]
[Route("api/registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _service;
    public RegistrationsController(IRegistrationService service) => _service = service;

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(string id)
    {
        var (success, message) = await _service.CancelAsync(id);
        if (!success) return NotFound(new { message });
        return Ok(new { message });
    }
}
