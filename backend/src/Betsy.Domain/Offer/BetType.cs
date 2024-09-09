using Betsy.Domain.Common;
using Throw;

namespace Betsy.Domain;

public sealed class BetType : EntityBase
{
    public Guid OfferId {get; private set; }

    public string Title { get; private set; } = string.Empty;
    public decimal Quota { get; private set; } 

    private BetType() { }

    public BetType(
        string title,
        decimal quota,
        Guid offerId,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        title.Throw("Invalid betting type title.").IfEmpty();
        quota.Throw("Invalid betting type quota.").IfLessThan(1);

        Title = title;
        Quota = quota;
        OfferId = offerId;
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