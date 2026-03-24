using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Management.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _service;
    public SubscriptionsController(ISubscriptionService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id}/use")]
    public async Task<IActionResult> UseSession(string id)
    {
        try
        {
            var result = await _service.UseSessionAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}
