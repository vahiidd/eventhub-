using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventHub.Event.Infrastructure.Persistence;
using EventHub.Event.Domain.Entities;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Event> Events => _database.GetCollection<Event>("events");
}