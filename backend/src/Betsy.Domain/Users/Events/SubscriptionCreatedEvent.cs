using Betsy.Domain;
using Betsy.Domain.Common;

namespace Newsy.Domain.Events;

public record UserRegisteredEvent(User user) : IDomainEvent;