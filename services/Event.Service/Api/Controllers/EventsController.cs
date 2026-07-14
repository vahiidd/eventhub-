using EventHub.Event.Application.Dtos;
using EventHub.Event.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Event.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly EventAppService _service;

    public EventsController(EventAppService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResult<EventDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? city = null, CancellationToken ct = default)
        => Ok(await _service.GetEventsAsync(page, pageSize, city, ct));

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(string id, CancellationToken ct)
        => Ok(await _service.GetEventByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventDto dto, CancellationToken ct)
    {
        var result = await _service.CreateEventAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _service.DeleteEventAsync(id, ct);
        return NoContent();
    }
}