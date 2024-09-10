using Betsy.Application.Common.Pagination;
using Betsy.Application.Offers.Common;
using ErrorOr;
using MediatR;

namespace Betsy.Application.Offers.Queries.GetAll;

public class GetOffersQuery : PaginationQuery, IRequest<ErrorOr<PaginationResult<OfferResponse>>>
{
    public bool SpecialOffers { get; set; }
    public string? Sport { get; set; }
}
