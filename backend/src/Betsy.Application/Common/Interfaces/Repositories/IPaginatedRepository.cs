using System.Linq.Expressions;
using Betsy.Application.Common.Pagination;
using Betsy.Domain.Common;

namespace Betsy.Application.Common.Interfaces.Repositories;

public interface IPaginatedRepository<T> : IBaseRepository<T> where T : EntityBase
{
    Task<PaginationResult<T>> GetPaginatedAsync(PaginationQuery pagination, Expression<Func<T, bool>>? whereExpression, CancellationToken token);
}
