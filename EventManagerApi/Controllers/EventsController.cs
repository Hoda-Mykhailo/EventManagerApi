using EventManagerApi.Models.DTO;
using EventManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _service.GetByIdAsync(id);
        return ev == null ? NotFound() : Ok(ev);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(EventDto dto)
    {
        var userId = int.Parse(User.FindFirstValue("userId")!);
        var ev = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EventDto dto)
    {
        var userId = int.Parse(User.FindFirstValue("userId")!);
        await _service.UpdateAsync(id, dto, userId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue("userId")!);
        await _service.DeleteAsync(id, userId);
        return NoContent();
    }
}
