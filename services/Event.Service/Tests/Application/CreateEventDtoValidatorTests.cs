using EventHub.Event.Application.Dtos;
using EventHub.Event.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace EventHub.Event.UnitTests.Application;

public class CreateEventDtoValidatorTests
{
    private readonly CreateEventDtoValidator _validator = new();

    [Fact]
    public void EmptyTitle_HasValidationError()
    {
        var dto = ValidDto() with { Title = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void DateInThePast_HasValidationError()
    {
        var dto = ValidDto() with { Date = DateTime.UtcNow.AddDays(-1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void NoTicketTypes_HasValidationError()
    {
        var dto = ValidDto() with { TicketTypes = new List<CreateTicketTypeDto>() };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.TicketTypes);
    }

    [Fact]
    public void NegativeTicketPrice_HasValidationError()
    {
        var dto = ValidDto() with
        {
            TicketTypes = new List<CreateTicketTypeDto> { new("Standard", -10, 100) }
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor("TicketTypes[0].Price");
    }

    [Fact]
    public void ValidDto_HasNoValidationErrors()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateEventDto ValidDto() => new(
        "Nuxt Nation Conf",
        "Ein Tag rund um Nuxt und Vue.",
        DateTime.UtcNow.AddMonths(1),
        new CreateVenueDto("Tech Hall", "Berlin"),
        new List<CreateTicketTypeDto> { new("Standard", 49, 120) });
}