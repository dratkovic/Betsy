using Betsy.Application.Common.Pagination;
using Betsy.Domain.Common;

namespace Betsy.Application.Common.Interfaces.Repositories;

public interface IPaginatedRepository<T> where T : EntityBase
{
    Task<PaginationResult<T>> GetPaginatedAsync(PaginationQuery pagination, IQueryable<T> query, CancellationToken token);
}
