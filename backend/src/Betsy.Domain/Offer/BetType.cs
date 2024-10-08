﻿using Betsy.Domain.Common;
using Throw;

namespace Betsy.Domain;

public sealed class BetType : EntityBase
{
    public Guid OfferId { get; private set; }
    public Match Match { get; private set; } = null!;
    public Guid MatchId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public decimal Quota { get; private set; }
    public short Order { get; private set; }

    public bool IsSpecialOffer { get; private set; }

    private BetType() { }

    public BetType(
        string title,
        decimal quota,
        Guid offerId,
        Guid matchId,
        bool isSpecialOffer = false,
        short order = 0,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        title.Throw("Invalid betting type title.").IfEmpty();

        MatchId = matchId;
        Title = title;
        Quota = quota;
        OfferId = offerId;
        IsSpecialOffer = isSpecialOffer;
        Order = order;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BetType other)
        {
            return false;
        }

        return Title == other.Title;
    }

    public override int GetHashCode()
    {
        return Title.GetHashCode();
    }
}