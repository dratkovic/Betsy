using FluentValidation;

namespace Betsy.Application.Tickets.Commands.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.TicketAmount)
            .GreaterThan(0);

        RuleFor(x => x.SelectedBetTypesIds)
            .NotEmpty();
    }
}
