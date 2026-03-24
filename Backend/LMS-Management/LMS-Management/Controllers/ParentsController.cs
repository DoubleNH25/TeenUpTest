using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Management.Controllers;

[ApiController]
[Route("api/parents")]
public class ParentsController : ControllerBase
{
    private readonly IParentService _service;
    public ParentsController(IParentService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateParentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
}
