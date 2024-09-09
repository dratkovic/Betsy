using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Infrastructure.Persistance.Users;
public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(BetsyDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken token)
    {
        var emailToLowerCase = email.ToLower();
        return await _dbContext.Users.AnyAsync(x => x.Email == emailToLowerCase, token);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
    {
        var emailToLowerCase = email.ToLower();
        return await _dbContext.Users.AsNoTracking().Where(x => x.Email == emailToLowerCase)
            .FirstOrDefaultAsync(token);
    }
}

