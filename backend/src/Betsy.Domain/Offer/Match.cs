using Betsy.Domain.Common;
using Throw;

namespace Betsy.Domain;

public sealed class Match : EntityBase
{
    public string NameOne { get; private set; } = string.Empty;
    public string? NameTwo { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }

    // if we will create the match from other service that handle matches
    // results and start/finish etc. we can use this property to correlate
    public string? CorrelationId { get; private set; }

    public string Sport { get; private set; } = string.Empty;

    private Match() { }

    public Match(
        string nameOne,
        string description,
        DateTime startsAtUtc,
        Sport sport,
        string? nameTwo = null,
        string? correlationId = null,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        nameOne.Throw("Invalid name").IfEmpty();
        description.Throw("Invalid description").IfEmpty();
        startsAtUtc.Throw("Invalid start date").IfLessThan(DateTime.UtcNow.AddMinutes(10));

        NameOne = nameOne;
        NameTwo = nameTwo;
        Description = description;
        StartsAtUtc = startsAtUtc;
        Sport = sport.ToString();
        CorrelationId = correlationId;
    }
}
