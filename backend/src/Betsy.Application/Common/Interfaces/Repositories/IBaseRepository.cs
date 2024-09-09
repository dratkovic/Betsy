using Betsy.Domain.Common;

namespace Betsy.Application.Common.Interfaces.Repositories;
public interface IBaseRepository<T> where T : EntityBase
{
    Task AddAsync(T entity, CancellationToken token = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(T entity, CancellationToken token = default);
}
