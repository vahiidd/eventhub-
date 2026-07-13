using EventHub.Event.Application.Abstractions;
using EventHub.Event.Application.Dtos;
using EventHub.Event.Application.Exceptions;
using EventHub.Event.Application.Mapping;

namespace EventHub.Event.Application.Services;

public class EventAppService
{
    private readonly IEventRepository _repository;

    public EventAppService(IEventRepository repository) => _repository = repository;

    public async Task<PagedResult<EventDto>> GetEventsAsync(int page, int pageSize, string? city, CancellationToken ct)
    {
        var (items, total) = await _repository.GetPagedAsync(page, pageSize, city, ct);
        return new PagedResult<EventDto>(items.Select(e => e.ToDto()).ToList(), total, page, pageSize);
    }

    public async Task<EventDto> GetEventByIdAsync(string id, CancellationToken ct)
    {
        var ev = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Event '{id}' wurde nicht gefunden.");
        return ev.ToDto();
    }

    public async Task<EventDto> CreateEventAsync(CreateEventDto dto, CancellationToken ct)
    {
        var entity = dto.ToDomain();
        await _repository.CreateAsync(entity, ct);
        return entity.ToDto();
    }

    public async Task DeleteEventAsync(string id, CancellationToken ct)
    {
        if (!await _repository.DeleteAsync(id, ct))
            throw new NotFoundException($"Event '{id}' wurde nicht gefunden.");
    }
}