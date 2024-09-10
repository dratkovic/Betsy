using System.Linq.Expressions;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Infrastructure.Common.Persistence;

namespace Betsy.Infrastructure.Persistance.Offer;
public class OfferRepository : PaginatedRepositoryBase<Domain.Offer>, IOfferRepository
{
    public OfferRepository(BetsyDbContext dbContext) : base(dbContext)
    {
    }

    protected override Expression<Func<Domain.Offer, bool>> GetFilterPredicate(string filter)
    {
        return x => x.Match.NameOne.Contains(filter) || 
                    (x.Match.NameTwo != null && x.Match.NameTwo.Contains(filter));
    }
    
    protected override IEnumerable<Expression<Func<Domain.Offer, object>>> GetDefaultIncludePredicates()
    {
        return new Expression<Func<Domain.Offer, object>>[]
        {
            x=>x.Match,
            x=>x.BetTypes
        };
    }
}

