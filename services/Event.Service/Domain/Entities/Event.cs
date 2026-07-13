using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventHub.Event.Domain.Entities;

public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public Venue Venue { get; set; } = new();
    public List<TicketType> TicketTypes { get; set; } = new();
}

public class Venue
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

public class TicketType
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Label { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Available { get; set; }
}