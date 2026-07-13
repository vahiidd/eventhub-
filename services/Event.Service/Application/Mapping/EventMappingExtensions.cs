using EventHub.Event.Application.Dtos;

namespace EventHub.Event.Application.Mapping;
using Domain = EventHub.Event.Domain.Entities;

public static class EventMappingExtensions
{
    public static EventDto ToDto(this Domain.Event e) =>
        new(e.Id, e.Title, e.Description, e.Date,
            new VenueDto(e.Venue.Id, e.Venue.Name, e.Venue.City),
            e.TicketTypes.Select(t => new TicketTypeDto(t.Id, t.Label, t.Price, t.Available)).ToList());

    public static Domain.Event ToDomain(this CreateEventDto dto) =>
        new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            Venue = new Domain.Venue { Name = dto.Venue.Name, City = dto.Venue.City },
            TicketTypes = dto.TicketTypes
                .Select(t => new Domain.TicketType { Label = t.Label, Price = t.Price, Available = t.Available })
                .ToList()
        };
}