namespace EventHub.Event.Application.Abstractions;
using EventHub.Event.Domain.Entities;

public interface IEventRepository
{
    Task<(IReadOnlyList<Event> Items, long Total)> GetPagedAsync(int page, int pageSize, string? city, CancellationToken ct);
    Task<Event?> GetByIdAsync(string id, CancellationToken ct);
    Task CreateAsync(Event ev, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}