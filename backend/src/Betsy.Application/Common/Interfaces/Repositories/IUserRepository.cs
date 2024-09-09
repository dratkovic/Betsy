using Betsy.Domain;

namespace Betsy.Application.Common.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
}