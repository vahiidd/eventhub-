using EventHub.Event.Application.Dtos;
using EventHub.Event.Application.Mapping;
using FluentAssertions;
using Xunit;

namespace EventHub.Event.UnitTests.Application;
using Domain = EventHub.Event.Domain.Entities;


public class EventMappingExtensionsTests
{
    [Fact]
    public void ToDto_MapsAllFieldsCorrectly()
    {
        var entity = new Domain.Event
        {
            Id = "1",
            Title = "Test Event",
            Description = "Beschreibung",
            Date = new DateTime(2026, 9, 12),
            Venue = new Domain.Venue { Id = "v1", Name = "Halle", City = "Berlin" },
            TicketTypes = new List<Domain.TicketType>
            {
                new() { Id = "t1", Label = "Standard", Price = 49, Available = 100 }
            }
        };

        var dto = entity.ToDto();

        dto.Id.Should().Be("1");
        dto.Venue.City.Should().Be("Berlin");
        dto.TicketTypes.Should().ContainSingle(t => t.Label == "Standard" && t.Price == 49);
    }

    [Fact]
    public void ToDomain_GeneratesIdsAndMapsFields()
    {
        var dto = new CreateEventDto(
            "Test Event",
            "Beschreibung",
            new DateTime(2026, 9, 12),
            new CreateVenueDto("Halle", "Berlin"),
            new List<CreateTicketTypeDto> { new("Standard", 49, 100) });

        var entity = dto.ToDomain();

        entity.Id.Should().NotBeNullOrWhiteSpace();
        entity.Venue.Id.Should().NotBeNullOrWhiteSpace();
        entity.TicketTypes.Single().Id.Should().NotBeNullOrWhiteSpace();
        entity.Venue.City.Should().Be("Berlin");
    }
}