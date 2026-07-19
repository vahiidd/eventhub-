using EventHub.Event.Application.Abstractions;
using EventHub.Event.Application.Dtos;
using EventHub.Event.Application.Exceptions;
using EventHub.Event.Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventHub.Event.UnitTests.Application;
using Domain = EventHub.Event.Domain.Entities;


public class EventAppServiceTests
{
    private readonly Mock<IEventRepository> _repositoryMock = new();
    private readonly EventAppService _sut; // "system under test"

    public EventAppServiceTests()
    {
        _sut = new EventAppService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetEventByIdAsync_EventExists_ReturnsMappedDto()
    {
        // Arrange
        var entity = CreateSampleEvent();
        _repositoryMock
            .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _sut.GetEventByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Title.Should().Be(entity.Title);
        result.TicketTypes.Should().HaveCount(entity.TicketTypes.Count);
    }

    [Fact]
    public async Task GetEventByIdAsync_EventMissing_ThrowsNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync("missing-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Event?)null);

        var act = () => _sut.GetEventByIdAsync("missing-id", CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateEventAsync_ValidDto_CallsRepositoryAndReturnsDto()
    {
        var dto = new CreateEventDto(
            "Nuxt Nation Conf",
            "Ein Tag rund um Nuxt und Vue.",
            DateTime.UtcNow.AddMonths(1),
            new CreateVenueDto("Tech Hall", "Berlin"),
            new List<CreateTicketTypeDto> { new("Standard", 49, 120) });

        Domain.Event? capturedEntity = null;
        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Domain.Event>(), It.IsAny<CancellationToken>()))
            .Callback<Domain.Event, CancellationToken>((e, _) => capturedEntity = e)
            .Returns(Task.CompletedTask);

        var result = await _sut.CreateEventAsync(dto, CancellationToken.None);

        result.Title.Should().Be(dto.Title);
        capturedEntity.Should().NotBeNull();
        capturedEntity!.Venue.City.Should().Be("Berlin");
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Domain.Event>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEventAsync_EventMissing_ThrowsNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.DeleteAsync("missing-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = () => _sut.DeleteEventAsync("missing-id", CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetEventsAsync_ReturnsPagedResultWithMappedItems()
    {
        var events = new List<Domain.Event> { CreateSampleEvent(), CreateSampleEvent() };
        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Items: (IReadOnlyList<Domain.Event>)events, Total: 2L));

        var result = await _sut.GetEventsAsync(1, 10, null, CancellationToken.None);

        result.Total.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    private static Domain.Event CreateSampleEvent() => new()
    {
        Title = "Nuxt Nation Conf",
        Description = "Ein Tag rund um Nuxt und Vue.",
        Date = DateTime.UtcNow.AddMonths(1),
        Venue = new Domain.Venue { Name = "Tech Hall", City = "Berlin" },
        TicketTypes = new List<Domain.TicketType>
        {
            new() { Label = "Standard", Price = 49, Available = 120 }
        }
    };
}