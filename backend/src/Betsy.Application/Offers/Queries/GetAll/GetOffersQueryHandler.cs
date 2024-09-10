using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Application.Common.Pagination;
using Betsy.Application.Offers.Common;
using ErrorOr;
using MediatR;
using Betsy.Domain;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Application.Offers.Queries.GetAll;

public class GetOffersQueryHandler : IRequestHandler<GetOffersQuery, ErrorOr<PaginationResult<OfferResponse>>>
{
    private readonly IBetsyDbContext _dbContext;
    private readonly IPaginatedRepository<Offer> _offerRepository;

    public GetOffersQueryHandler(IBetsyDbContext dbContext, IPaginatedRepository<Offer> offerRepository)
    {
        _dbContext = dbContext;
        _offerRepository = offerRepository;
    }

    public async Task<ErrorOr<PaginationResult<OfferResponse>>> Handle(GetOffersQuery request, CancellationToken cancellationToken)
    {
        var fiveMinutesFromNow = DateTime.UtcNow.AddMinutes(5);

        var query = _dbContext.Set<Offer>()
            .Include(x => x.Match)
            .Include(x => x.BetTypes)
            .Where(x => !x.IsDeleted &&
                        x.IsSpecialOffer == request.SpecialOffers &&
                        x.Match.StartsAtUtc > fiveMinutesFromNow);

        if (request.Sport != null)
        {
            query = query.Where(x => x.Match.Sport == request.Sport);
        }

        if (request.Filter is not null)
        {
            query = query.Where(x => x.Match.NameOne.Contains(request.Filter) ||
                       (x.Match.NameTwo != null && x.Match.NameTwo.Contains(request.Filter)));
        }

        query = query.OrderByDescending(x => x.Match.StartsAtUtc);

        var result = await _offerRepository.GetPaginatedAsync(request,
            query,
            cancellationToken);

        var data = result.Data.Select(x => x.ToOfferResponse()).ToList();

        return new PaginationResult<OfferResponse>(data, result.TotalRecords, result.Page, result.PageSize);
    }
}
