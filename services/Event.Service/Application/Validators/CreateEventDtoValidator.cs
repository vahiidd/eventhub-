using EventHub.Event.Application.Dtos;
using FluentValidation;

namespace EventHub.Event.Application.Validators;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow).WithMessage("Datum muss in der Zukunft liegen.");
        RuleFor(x => x.Venue).NotNull();
        RuleFor(x => x.TicketTypes).NotEmpty().WithMessage("Mindestens ein Tickettyp erforderlich.");
        RuleForEach(x => x.TicketTypes).ChildRules(t =>
        {
            t.RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            t.RuleFor(x => x.Available).GreaterThanOrEqualTo(0);
        });
    }
}