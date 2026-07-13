using EventHub.Event.Application.Abstractions;
using EventHub.Event.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EventHub.Event.Infrastructure.Repositories;
using EventHub.Event.Domain.Entities;

public class EventRepository : IEventRepository
{
    private readonly MongoDbContext _context;

    public EventRepository(MongoDbContext context) => _context = context;

    public async Task<(IReadOnlyList<Event> Items, long Total)> GetPagedAsync(int page, int pageSize, string? city, CancellationToken ct)
    {
        var filter = string.IsNullOrWhiteSpace(city)
            ? Builders<Event>.Filter.Empty
            : Builders<Event>.Filter.Eq(e => e.Venue.City, city);

        var total = await _context.Events.CountDocumentsAsync(filter, cancellationToken: ct);
        var items = await _context.Events.Find(filter)
            .SortBy(e => e.Date)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Event?> GetByIdAsync(string id, CancellationToken ct) =>
        _context.Events.Find(e => e.Id == id).FirstOrDefaultAsync(ct)!;

    public Task CreateAsync(Event ev, CancellationToken ct) =>
        _context.Events.InsertOneAsync(ev, cancellationToken: ct);

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _context.Events.DeleteOneAsync(e => e.Id == id, ct);
        return result.DeletedCount > 0;
    }
}