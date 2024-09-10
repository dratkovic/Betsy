using Betsy.Application.Tickets.Commands;
using FluentValidation;

namespace Betsy.Application.Offers.Queries.GetAll;

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
