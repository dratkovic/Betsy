using Betsy.Domain.Common;
using Betsy.Domain;

namespace Newsy.Domain.Events;

public record UserRegisteredEvent(User user) : IDomainEvent;