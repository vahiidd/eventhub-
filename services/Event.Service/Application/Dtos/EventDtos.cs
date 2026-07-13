namespace EventHub.Event.Application.Dtos;

public record VenueDto(string Id, string Name, string City);
public record TicketTypeDto(string Id, string Label, decimal Price, int Available);
public record EventDto(string Id, string Title, string Description, DateTime Date, VenueDto Venue, List<TicketTypeDto> TicketTypes);

public record CreateVenueDto(string Name, string City);
public record CreateTicketTypeDto(string Label, decimal Price, int Available);
public record CreateEventDto(string Title, string Description, DateTime Date, CreateVenueDto Venue, List<CreateTicketTypeDto> TicketTypes);

public record PagedResult<T>(IReadOnlyList<T> Items, long Total, int Page, int PageSize);